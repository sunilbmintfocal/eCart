using MintCart.Api.Business.Reports.Interface;
using MintCart.Api.Business.Reports.Model;
using MintCart.Api.Domain.Reports.Interfaces;

namespace MintCart.Api.Business.Reports.Interactor
{
    public class ReportInteractor : IReportInteractor
    {
        private readonly IReportRepository _reportRepository;

        public ReportInteractor(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<DailySalesReportModel>> GetDailySalesReportAsync(DateTime? fromDate, DateTime? toDate, string? company)
        {
            // Date filtering is handled by the SP; company is filtered in-memory
            var entities = await _reportRepository.GetDailySalesAsync(fromDate, toDate);

            var query = entities.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(company))
                query = query.Where(r => r.Company!.Equals(company, StringComparison.OrdinalIgnoreCase));

            return query
                .Select((r, index) => new DailySalesReportModel
                {
                    SlNo           = index + 1,
                    Company        = r.Company,
                    ItemCode       = r.ItemCode,
                    ItemName       = r.ItemName,
                    InvoiceNo      = r.InvoiceNo,
                    SaleDateString = r.SaleDateString,
                    Rate           = r.Rate,
                    Quantity       = r.Quantity,
                    TaxableAmount  = r.TaxableAmount,
                    GSTRate        = r.GSTRate,
                    TotalGSTAmount = r.TotalGSTAmount,
                    TotalAmount    = r.TotalAmount,
                })
                .ToList();
        }
    }
}
