using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Sale.Entities;
using MintCart.Api.Domain.Sale.Interfaces;

namespace MintCart.Api.Data.Sale.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SaleDbContext _context;

        public SaleRepository(SaleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleEntity>> GetTodaysSalesAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Sales
                .Where(s => s.SaleDate >= today && s.SaleDate < tomorrow)
                .ToListAsync();
        }

        public async Task<IEnumerable<SaleEntity>> GetYesterdaysSalesAsync()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            return await _context.Sales
                .Where(s => s.SaleDate >= yesterday && s.SaleDate < today)
                .ToListAsync();
        }

        public async Task<IEnumerable<SaleEntity>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.Sales
                .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate && s.IsCancelled != true)
                .ToListAsync();
        }
    }
}
