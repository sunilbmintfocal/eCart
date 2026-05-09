using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Sale.Entities;
using MintCart.Api.Domain.Sale.Interfaces;

namespace MintCart.Api.Data.Sale.Repository
{
    /// <summary>
    /// Repository for handling sale persistence operations.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        private readonly SaleDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleRepository"/> class.
        /// </summary>
        /// <param name="context">The sale database context.</param>
        public SaleRepository(SaleDbContext context)
        {
            _context = context;
        }

        #region Public Methods
        /// <summary>
        /// Retrieves sales from the current date.
        /// </summary>
        /// <returns>A collection of SaleEntity objects.</returns>
        public async Task<IEnumerable<SaleEntity>> GetTodaysSalesAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Sales
                .AsNoTracking()
                .Where(s => s.SaleDate >= today && s.SaleDate < tomorrow)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves sales from the previous date.
        /// </summary>
        /// <returns>A collection of SaleEntity objects.</returns>
        public async Task<IEnumerable<SaleEntity>> GetYesterdaysSalesAsync()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            return await _context.Sales
                .AsNoTracking()
                .Where(s => s.SaleDate >= yesterday && s.SaleDate < today)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves sales within a specific date range.
        /// </summary>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>A collection of SaleEntity objects filtered by date range.</returns>
        public async Task<IEnumerable<SaleEntity>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.Sales
                .AsNoTracking()
                .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate && s.IsCancelled != true)
                .ToListAsync();
        }
        #endregion
    }
}
