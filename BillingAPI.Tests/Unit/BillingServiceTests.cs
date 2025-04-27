using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;
using BillingAPI.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BillingAPI.Tests.Unit
{
    public class BillingServiceTests
    {
        private readonly Mock<IPaymentGatewayFactory> _mockGatewayFactory;
        private readonly Mock<IReceiptRepository> _mockReceiptRepository;
        private readonly Mock<ILogger<BillingService>> _mockLogger;
        private readonly BillingService _service;

        public BillingServiceTests()
        {
            _mockGatewayFactory = new Mock<IPaymentGatewayFactory>();
            _mockReceiptRepository = new Mock<IReceiptRepository>();
            _mockLogger = new Mock<ILogger<BillingService>>();
            _service = new BillingService(_mockGatewayFactory.Object, _mockReceiptRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessOrder_Should_ReturnReceipt_WhenPaymentSuccessful()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderNumber = "ORD123",
                UserId = "USER456",
                PayableAmount = 100.00m,
                PaymentGateway = "TestGateway",
                Description = "Test order"
            };

            var paymentResult = new PaymentResult
            {
                IsSuccess = true,
                TransactionId = "TXN12345"
            };

            var mockGateway = new Mock<IPaymentGateway>();
            mockGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<OrderRequest>())).ReturnsAsync(paymentResult);

            _mockGatewayFactory.Setup(x => x.GetGateway(It.IsAny<string>())).Returns(mockGateway.Object);

            _mockReceiptRepository.Setup(x => x.AddAsync(It.IsAny<Receipt>())).Returns(Task.CompletedTask);

            _mockReceiptRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.ProcessOrderAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderRequest.OrderNumber, result.OrderNumber);
            Assert.Equal(orderRequest.UserId, result.UserId);
            Assert.Equal(orderRequest.PayableAmount, result.Amount);
            Assert.Equal(paymentResult.TransactionId, result.TransactionId);

            _mockReceiptRepository.Verify(x => x.AddAsync(It.IsAny<Receipt>()), Times.Once);
            _mockReceiptRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ProcessOrder_Should_ThrowException_WhenPaymentFails()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderNumber = "ORD123",
                UserId = "USER456",
                PayableAmount = 100.00m,
                PaymentGateway = "TestGateway",
                Description = "Test order"
            };

            var paymentResult = new PaymentResult
            {
                IsSuccess = false,
                Message = "Insufficient funds"
            };

            var mockGateway = new Mock<IPaymentGateway>();
            mockGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<OrderRequest>())).ReturnsAsync(paymentResult);

            _mockGatewayFactory.Setup(x => x.GetGateway(It.IsAny<string>())).Returns(mockGateway.Object);

            // Act
            var exception = await Assert.ThrowsAsync<PaymentProcessingException>(() => _service.ProcessOrderAsync(orderRequest));

            // Assert
            Assert.Contains("Insufficient funds", exception.Message);

            _mockReceiptRepository.Verify(x => x.AddAsync(It.IsAny<Receipt>()), Times.Never);
            _mockReceiptRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(null, "USER456", 100.00, "Gateway")]
        [InlineData("ORD123", null, 100.00, "Gateway")]
        [InlineData("ORD123", "USER456", 0, "Gateway")]
        [InlineData("ORD123", "USER456", -10.00, "Gateway")]
        [InlineData("ORD123", "USER456", 100.00, null)]
        public async Task ProcessOrder_Should_ThrowValidationException_WhenRequestInvalid(string orderNumber, string userId, decimal amount, string gateway)
        {
            // Arrange
            var invalidRequest = new OrderRequest
            {
                OrderNumber = orderNumber,
                UserId = userId,
                PayableAmount = amount,
                PaymentGateway = gateway
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.ProcessOrderAsync(invalidRequest));
        }
    }
}