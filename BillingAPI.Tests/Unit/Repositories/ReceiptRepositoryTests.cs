using BillingAPI.Core.Models;
using BillingAPI.Infrastructure.Data;
using BillingAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BillingAPI.Tests.Unit.Repositories
{
    public class ReceiptRepositoryTests
    {
        private readonly BillingDbContext _dbContext;
        private readonly Mock<ILogger<ReceiptRepository>> _mockLogger;
        private readonly ReceiptRepository _receiptRepository;

        public ReceiptRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BillingDbContext>().UseInMemoryDatabase(databaseName: "BillingAPI_Dev").Options;

            _dbContext = new BillingDbContext(options);
            _mockLogger = new Mock<ILogger<ReceiptRepository>>();
            _receiptRepository = new ReceiptRepository(_dbContext);
        }

        [Fact]
        public async Task AddAsync_Should_AddReceiptToDbSet()
        {
            // Arrange
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = "ORD123",
                UserId = "USER456",
                Amount = 100.00m,
                PaymentGateway = "TestGateway",
                ProcessedDate = DateTime.UtcNow,
                TransactionId = "TXN12345",
                Description = "Test receipt"
            };

            // Act
            await _receiptRepository.AddAsync(receipt);
            await _receiptRepository.SaveChangesAsync();

            // Assert
            var addedReceipt = await _dbContext.Receipts.FirstOrDefaultAsync(r => r.OrderNumber == "ORD123");
            Assert.NotNull(addedReceipt);
            Assert.Equal(receipt.OrderNumber, addedReceipt.OrderNumber);
        }

        [Fact]
        public async Task GetByOrderNumberAsync_Should_ReturnReceipt_WhenReceiptExists()
        {
            // Arrange
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = "ORD123",
                UserId = "USER456",
                Amount = 100.00m,
                PaymentGateway = "TestGateway",
                ProcessedDate = DateTime.UtcNow,
                TransactionId = "TXN12345",
                Description = "Test receipt"
            };

            _dbContext.Receipts.Add(receipt);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _receiptRepository.GetByOrderNumberAsync("ORD123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receipt.OrderNumber, result.OrderNumber);
        }

        [Fact]
        public async Task GetByOrderNumberAsync_Should_ReturnNull_WhenReceiptDoesNotExist()
        {
            // Act
            var result = await _receiptRepository.GetByOrderNumberAsync("ORD127");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task OrderNumberExistsAsync_Should_ReturnTrue_WhenOrderNumberExists()
        {
            // Arrange
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = "ORD123",
                UserId = "USER456",
                Amount = 100.00m,
                PaymentGateway = "TestGateway",
                ProcessedDate = DateTime.UtcNow,
                TransactionId = "TXN12345",
                Description = "Test receipt"
            };

            _dbContext.Receipts.Add(receipt);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _receiptRepository.OrderNumberExistsAsync("ORD123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task OrderNumberExistsAsync_Should_ReturnFalse_WhenOrderNumberDoesNotExist()
        {
            // Act
            var result = await _receiptRepository.OrderNumberExistsAsync("ORD127");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SaveChangesAsync_Should_CallSaveChangesOnDbContext()
        {
            // Arrange
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = "ORD123",
                UserId = "USER456",
                Amount = 100.00m,
                PaymentGateway = "TestGateway",
                ProcessedDate = DateTime.UtcNow,
                TransactionId = "TXN12345",
                Description = "Test receipt"
            };

            _dbContext.Receipts.Add(receipt);

            // Act
            await _receiptRepository.SaveChangesAsync();

            // Assert
            var savedReceipt = await _dbContext.Receipts.FirstOrDefaultAsync(r => r.OrderNumber == "ORD123");
            Assert.NotNull(savedReceipt);
        }
    }
}
