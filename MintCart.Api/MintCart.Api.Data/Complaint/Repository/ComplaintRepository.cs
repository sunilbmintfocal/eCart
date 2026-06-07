using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Complaint.Interfaces;

namespace MintCart.Api.Data.Complaint.Repository
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly ComplaintDbContext _context;

        public ComplaintRepository(ComplaintDbContext context)
        {
            _context = context;
        }

        public async Task UpdateCustomerIdAsync(List<int> fromCustomerIds, int toCustomerId)
        {
            foreach (var fromId in fromCustomerIds)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE ComplaintReg SET intCustomerId = {toCustomerId} WHERE intCustomerId = {fromId}");
            }
        }
    }
}
