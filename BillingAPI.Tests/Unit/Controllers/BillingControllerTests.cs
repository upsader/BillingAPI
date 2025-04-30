using BillingAPI.API.Controllers;
using BillingAPI.Core.Exceptions;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BillingAPI.Tests.Unit.Controllers
{
    public class BillingControllerTests
    {
        private readonly Mock<IBillingService> _mockBillingService;
        private readonly Mock<ILogger<BillingController>> _mockLogger;
        private readonly BillingController _controller;

        public BillingControllerTests()
        {
            _mockBillingService = new Mock<IBillingService>();
            _mockLogger = new Mock<ILogger<BillingController>>();
            _controller = new BillingController(_mockBillingService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessOrder_Should_ReturnOk_WhenOrderProcessedSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: "ORD123",
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "TestGateway",
                Description: "Test order"
            );

            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = orderRequest.OrderNumber,
                UserId = orderRequest.UserId,
                Amount = orderRequest.PayableAmount,
                PaymentGateway = orderRequest.PaymentGateway,
                ProcessedDate = DateTime.UtcNow,
                TransactionId = "TXN12345",
                Description = orderRequest.Description
            };

            _mockBillingService.Setup(x => x.ProcessOrderAsync(orderRequest)).ReturnsAsync(receipt);

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedReceipt = Assert.IsType<Receipt>(okResult.Value);
            Assert.Equal(orderRequest.OrderNumber, returnedReceipt.OrderNumber);
            Assert.Equal(orderRequest.UserId, returnedReceipt.UserId);
            Assert.Equal(orderRequest.PayableAmount, returnedReceipt.Amount);
        }

        [Fact]
        public async Task ProcessOrder_Should_CallService_WithCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: "ORD123",
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "TestGateway",
                Description: "Test order"
            );

            _mockBillingService.Setup(x => x.ProcessOrderAsync(orderRequest)).ReturnsAsync(It.IsAny<Receipt>());

            // Act
            await _controller.ProcessOrder(orderRequest);

            // Assert
            _mockBillingService.Verify(x => x.ProcessOrderAsync(orderRequest), Times.Once);
        }

        [Fact]
        public async Task ProcessOrder_Should_ThrowValidationException()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: null,
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "TestGateway",
                Description: "Test order"
            );

            _mockBillingService.Setup(x => x.ProcessOrderAsync(orderRequest)).ThrowsAsync(new ValidationException("Order number is required"));

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.ProcessOrder(orderRequest));

            // Assert
            Assert.Equal("Order number is required", exception.Message);
        }

        [Fact]
        public async Task ProcessOrder_Should_ThrowPaymentProcessingException()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: "ORD123",
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "TestGateway",
                Description: "Test order"
            );

            _mockBillingService.Setup(x => x.ProcessOrderAsync(orderRequest)).ThrowsAsync(new PaymentProcessingException($"Payment failed for order {orderRequest.OrderNumber}"));

            // Act
            var exception = await Assert.ThrowsAsync<PaymentProcessingException>(() => _controller.ProcessOrder(orderRequest));

            // Assert
            Assert.Equal($"Payment failed for order {orderRequest.OrderNumber}", exception.Message);
        }

        [Fact]
        public async Task ProcessOrder_Should_ThrowUnhandledException()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: "ORD123",
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "TestGateway",
                Description: "Test order"
            );

            _mockBillingService.Setup(x => x.ProcessOrderAsync(orderRequest)).ThrowsAsync(new Exception("Unhandled exception"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.ProcessOrder(orderRequest));

            // Assert
            Assert.Equal("Unhandled exception", exception.Message);
        }

        [Fact]
        public async Task GetReceipt_Should_ReturnOk_WhenReceiptExists()
        {
            // Arrange
            var orderNumber = "ORD123";
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = orderNumber,
                UserId = "USER456",
                Amount = 100.00m,
                PaymentGateway = "TestGateway",
                ProcessedDate = DateTime.UtcNow,
                TransactionId = "TXN12345",
                Description = "Test receipt"
            };

            _mockBillingService.Setup(x => x.GetReceiptAsync(orderNumber)).ReturnsAsync(receipt);

            // Act
            var result = await _controller.GetReceipt(orderNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedReceipt = Assert.IsType<Receipt>(okResult.Value);
            Assert.Equal(orderNumber, returnedReceipt.OrderNumber);
        }

        [Fact]
        public async Task GetReceipt_Should_CallService_WithCorrectParameters()
        {
            // Arrange
            var orderNumber = "ORD123";

            _mockBillingService.Setup(x => x.GetReceiptAsync(orderNumber)).ReturnsAsync(It.IsAny<Receipt>());

            // Act
            await _controller.GetReceipt(orderNumber);

            // Assert
            _mockBillingService.Verify(x => x.GetReceiptAsync(orderNumber), Times.Once);
        }

        [Fact]
        public async Task GetReceipt_Should_ThrowNotFoundException()
        {
            // Arrange
            var orderNumber = "ORD123";

            _mockBillingService.Setup(x => x.GetReceiptAsync(orderNumber)).ThrowsAsync(new NotFoundException($"Receipt for order {orderNumber} not found"));

            // Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetReceipt(orderNumber));

            // Assert
            Assert.Equal($"Receipt for order {orderNumber} not found", exception.Message);
        }

        [Fact]
        public async Task GetReceipt_Should_ThrowUnhandledException()
        {
            // Arrange
            var orderNumber = "ORD123";

            _mockBillingService.Setup(x => x.GetReceiptAsync(orderNumber)).ThrowsAsync(new Exception("Unhandled exception"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetReceipt(orderNumber));

            // Assert
            Assert.Equal("Unhandled exception", exception.Message);
        }
    }
}
