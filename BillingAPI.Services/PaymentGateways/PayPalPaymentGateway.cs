using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;

namespace BillingAPI.Services.PaymentGateways
{
    public class PayPalPaymentGateway : IPaymentGateway
    {
        public async Task<PaymentResult> ProcessPaymentAsync(OrderRequest order)
        {
            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = $"ch_{Guid.NewGuid()}"
            };
        }
    }
}