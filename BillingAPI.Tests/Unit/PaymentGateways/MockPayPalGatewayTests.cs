using BillingAPI.Core.Models;
using Xunit;

namespace BillingAPI.Tests.Unit.PaymentGateways
{
    public class MockPayPalGatewayTests
    {
        private readonly MockPayPalGateway _gateway;

        public MockPayPalGatewayTests()
        {
            _gateway = new MockPayPalGateway();
        }

        [Fact]
        public async Task ProcessPaymentAsync_Should_ReturnSuccessfulPaymentResult()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: "ORD123",
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "PayPal",
                Description: "Test order"
            );

            // Act
            var result = await _gateway.ProcessPaymentAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.StartsWith("MOCK-PAYPAL-", result.TransactionId);
            Assert.Equal("Mock PayPal payment succeeded", result.Message);
        }
    }
}
