using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Sale.Entities;
using Microsoft.Extensions.Configuration;

namespace MintCart.Api.Data.Sale
{
    public class SaleDbContext : DbContext
    {
        public SaleDbContext(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<SaleChildEntity> SaleChildren { get; set; }
        public DbSet<SaleTransactionEntity> SaleTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Map entities to tables
            // Primary key mappings are already defined via data annotations in entities
        }
    }
}
