using MintCart.Api.Business.Reports.Model;

namespace MintCart.Api.Business.Reports.Interface
{
    public interface IReportInteractor
    {
        Task<List<DailySalesReportModel>> GetDailySalesReportAsync(DateTime? fromDate, DateTime? toDate, string? company);
    }
}
