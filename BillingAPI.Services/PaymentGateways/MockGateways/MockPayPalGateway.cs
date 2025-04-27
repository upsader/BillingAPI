using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;

public class MockPayPalGateway : IPaymentGateway
{
    public async Task<PaymentResult> ProcessPaymentAsync(OrderRequest order)
    {
        await Task.Delay(150);
        return new PaymentResult
        {
            IsSuccess = true,
            TransactionId = $"MOCK-PAYPAL-{Guid.NewGuid()}",
            Message = "Mock PayPal payment succeeded"
        };
    }
}