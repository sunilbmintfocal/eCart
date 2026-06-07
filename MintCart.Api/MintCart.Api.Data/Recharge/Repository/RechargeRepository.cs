using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Recharge.Interfaces;

namespace MintCart.Api.Data.Recharge.Repository
{
    public class RechargeRepository : IRechargeRepository
    {
        private readonly RechargeDbContext _context;

        public RechargeRepository(RechargeDbContext context)
        {
            _context = context;
        }

        public async Task UpdateCustomerIdAsync(List<int> fromCustomerIds, int toCustomerId)
        {
            foreach (var fromId in fromCustomerIds)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE Recharge SET intCustomerId = {toCustomerId} WHERE intCustomerId = {fromId}");
            }
        }
    }
}
