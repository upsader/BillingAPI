using BillingAPI.Core.Exceptions;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;
using BillingAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Infrastructure.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly BillingDbContext _context;

        public ReceiptRepository(BillingDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Receipt receipt) => await _context.Receipts.AddAsync(receipt);

        public async Task<Receipt> GetByOrderNumberAsync(string orderNumber) => await _context.Receipts.FirstOrDefaultAsync(r => r.OrderNumber == orderNumber);

        public async Task<bool> OrderNumberExistsAsync(string orderNumber) => await _context.Receipts.AnyAsync(r => r.OrderNumber == orderNumber);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
