using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;

public class MockStripeGateway : IPaymentGateway
{
    public async Task<PaymentResult> ProcessPaymentAsync(OrderRequest order)
    {
        await Task.Delay(150);
        return new PaymentResult
        {
            IsSuccess = true,
            TransactionId = $"MOCK-STRIPE-{Guid.NewGuid()}",
            Message = "Mock Stripe payment succeeded"
        };
    }
}