using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Recharge.Entities;

namespace MintCart.Api.Data.Recharge
{
    public class RechargeDbContext : DbContext
    {
        public DbSet<RechargeEntity> Recharges { get; set; }

        public RechargeDbContext(DbContextOptions<RechargeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
