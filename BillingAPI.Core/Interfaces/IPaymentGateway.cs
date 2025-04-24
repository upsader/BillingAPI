using BillingAPI.Core.Models;

namespace BillingAPI.Core.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> ProcessPaymentAsync(OrderRequest order);
    }
}
