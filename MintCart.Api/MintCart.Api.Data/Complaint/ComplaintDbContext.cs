using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Complaint.Entities;

namespace MintCart.Api.Data.Complaint
{
    public class ComplaintDbContext : DbContext
    {
        public DbSet<ComplaintRegEntity> ComplaintRegs { get; set; }

        public ComplaintDbContext(DbContextOptions<ComplaintDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
