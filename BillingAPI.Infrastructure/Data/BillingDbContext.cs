using BillingAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Infrastructure.Data
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options) { }

        public virtual DbSet<Receipt> Receipts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });
        }
    }
}