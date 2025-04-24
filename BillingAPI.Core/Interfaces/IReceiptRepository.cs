using BillingAPI.Core.Models;

namespace BillingAPI.Core.Interfaces
{
    public interface IReceiptRepository
    {
        Task AddAsync(Receipt receipt);
        Task<Receipt> GetByOrderNumberAsync(string orderNumber);
        Task SaveChangesAsync();
        Task<bool> OrderNumberExistsAsync(string orderNumber);
    }
}