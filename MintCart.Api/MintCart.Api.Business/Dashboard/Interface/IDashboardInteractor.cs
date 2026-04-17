using MintCart.Api.Dashboard.Business.Model;

namespace MintCart.Api.Dashboard.Business.Interface
{
    public interface IDashboardInteractor
    {
        Task<DashboardMetricsModel> GetDashboardMetricsAsync();
        Task<DashboardKpiModel> GetKpisAsync();
        Task<DashboardPayablesModel> GetPayablesAsync();
        Task<List<DashboardActivityModel>> GetRecentActivitiesAsync(int topN = 10);
        Task<DashboardSalesTrendModel> GetSalesTrendAsync();
    }
}
