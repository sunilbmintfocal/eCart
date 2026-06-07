using MintCart.Api.Domain.Reports.Entities;

namespace MintCart.Api.Domain.Reports.Interfaces
{
    public interface IReportRepository
    {
        Task<List<SaleDetailEntity>> GetDailySalesAsync(DateTime? fromDate, DateTime? toDate);
    }
}
