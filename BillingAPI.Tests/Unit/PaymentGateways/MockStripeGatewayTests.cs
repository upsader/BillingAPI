using BillingAPI.Core.Models;
using Xunit;

namespace BillingAPI.Tests.Unit.PaymentGateways
{
    public class MockStripeGatewayTests
    {
        private readonly MockStripeGateway _gateway;

        public MockStripeGatewayTests()
        {
            _gateway = new MockStripeGateway();
        }

        [Fact]
        public async Task ProcessPaymentAsync_Should_ReturnSuccessfulPaymentResult()
        {
            // Arrange
            var orderRequest = new OrderRequest(
                OrderNumber: "ORD123",
                UserId: "USER456",
                PayableAmount: 100.00m,
                PaymentGateway: "Stripe",
                Description: "Test order"
            );

            // Act
            var result = await _gateway.ProcessPaymentAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.StartsWith("MOCK-STRIPE-", result.TransactionId);
            Assert.Equal("Mock Stripe payment succeeded", result.Message);
        }
    }
}
