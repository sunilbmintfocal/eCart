using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Reports.Entities;
using MintCart.Api.Domain.Reports.Interfaces;
using System.Globalization;

namespace MintCart.Api.Data.Reports.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportDbContext _context;

        public ReportRepository(ReportDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executes [dbo].[sprptSaleDetails1] @FrmDate, @ToDate, @ItemCodeP.
        /// Dates are passed as varchar in 'dd-MMM-yyyy' format (e.g. '01-Apr-2026').
        /// Pass empty string for @ItemCodeP to return all items.
        /// </summary>
        public async Task<List<SaleDetailEntity>> GetDailySalesAsync(DateTime? fromDate, DateTime? toDate)
        {
            var frmDate = fromDate.HasValue
                ? fromDate.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
                : string.Empty;

            var toDateStr = toDate.HasValue
                ? toDate.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
                : string.Empty;

            var frmParam      = new SqlParameter("@FrmDate",   frmDate);
            var toParam       = new SqlParameter("@ToDate",    toDateStr);
            var itemCodeParam = new SqlParameter("@ItemCodeP", string.Empty);

            return await _context.SaleDetails
                .FromSqlRaw("EXEC [dbo].[sprptSaleDetails1] @FrmDate, @ToDate, @ItemCodeP", frmParam, toParam, itemCodeParam)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
