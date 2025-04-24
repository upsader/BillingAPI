using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;
using Moq;

public class BillingServiceTests
{
    [Fact]
    public async Task ProcessOrder_Should_ReturnReceipt_WhenPaymentSuccessful()
    {
        // Arrange
        var mockGateway = new Mock<IPaymentGateway>();
        mockGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<OrderRequest>()))
            .ReturnsAsync(new PaymentResult { IsSuccess = true });

        var service = new BillingService(mockGateway.Object);

        // Act
        var result = await service.ProcessOrderAsync(new OrderRequest());

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Receipt>(result);
    }
}