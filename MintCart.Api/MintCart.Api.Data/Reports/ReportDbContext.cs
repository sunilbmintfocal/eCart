using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Reports.Entities;

namespace MintCart.Api.Data.Reports
{
    public class ReportDbContext : DbContext
    {
        public DbSet<SaleDetailEntity> SaleDetails { get; set; }

        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SaleDetailEntity is a keyless projection — it maps to stored procedure output, not a table.
            modelBuilder.Entity<SaleDetailEntity>().HasNoKey();
        }
    }
}
