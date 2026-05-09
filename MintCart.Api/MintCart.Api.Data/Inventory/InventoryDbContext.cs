using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Inventory.Entities;

namespace MintCart.Api.Data.Inventory
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<ItemMasterEntity> ItemMasters { get; set; }
        public DbSet<ItemStockEntity> ItemStocks { get; set; }
        public DbSet<ItemStockSummaryEntity> ItemStockSummaries { get; set; }
        public DbSet<ItemStockTransEntity> ItemStockTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // ItemStock Decimal precision configuration
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numStockQty).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numStockRetQty).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numTotalStockQty).HasPrecision(18, 3);
            
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numPurchase_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numPurchase_WithoutGST_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numPurchase_GST_Amount).HasPrecision(18, 2);
            
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numPurchase_Audit_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numPurchase_Audit_WithoutGST_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numPurchase_Audit_GST_Amount).HasPrecision(18, 2);
            
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numSelling_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numSelling_WithoutGST_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numSelling_GST_Amount).HasPrecision(18, 2);
            
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numSelling_Audit_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numSelling_Audit_WithoutGST_Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ItemStockEntity>().Property(p => p.numSelling_Audit_GST_Amount).HasPrecision(18, 2);

            // ItemStockSummary Decimal precision
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numStock).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numStockReturn).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numTotalStok).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numSale).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numSalesReturn).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numTotalSale).HasPrecision(18, 3);
            modelBuilder.Entity<ItemStockSummaryEntity>().Property(p => p.numInStock).HasPrecision(18, 3);

            // ItemStockTrans Decimal precision
            modelBuilder.Entity<ItemStockTransEntity>().Property(p => p.numQtyTrans).HasPrecision(18, 3);
        }
    }
}
