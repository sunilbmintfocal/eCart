using Microsoft.EntityFrameworkCore;
using MintCart.Api.Customer.Domain.Entities.Customer;
using Microsoft.Extensions.Configuration;

namespace MintCart.Api.Customer.Data
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        private readonly string? _schema;

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IConfiguration configuration) : base(options)
        {
            _schema = configuration.GetValue<string>("DatabaseSchema");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema(_schema ?? "dbo");
            
            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(e => e.Id);
            });
        }
    }
}
