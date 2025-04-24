using BillingAPI.Core.Models;

namespace BillingAPI.Core.Interfaces
{
    public interface IBillingService
    {
        Task<Receipt> ProcessOrderAsync(OrderRequest order);
        Task<Receipt> GetReceiptAsync(string orderNumber);
    }
}
