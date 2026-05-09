using MintCart.Api.Dashboard.Business.Interface;
using MintCart.Api.Dashboard.Business.Model;
using MintCart.Api.Domain.Sale.Interfaces;
using MintCart.Api.Business.Inventory.Interface;
using MintCart.Common;
using System.Linq;

namespace MintCart.Api.Dashboard.Business.Interactor
{
    /// <summary>
    /// Handles the business logic for the Dashboard.
    /// </summary>
    public class DashboardInteractor : IDashboardInteractor
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IInventoryInteractor _inventoryInteractor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardInteractor"/> class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="inventoryInteractor">The inventory interactor.</param>
        public DashboardInteractor(ISaleRepository saleRepository, IInventoryInteractor inventoryInteractor)
        {
            _saleRepository = saleRepository;
            _inventoryInteractor = inventoryInteractor;
        }

        #region Public Methods
        /// <summary>
        /// Retrieves all aggregated dashboard metrics in a single call.
        /// </summary>
        /// <returns>A model containing KPIs, payables, activities, and sales trends.</returns>
        public async Task<DashboardMetricsModel> GetDashboardMetricsAsync()
        {
            try
            {
                // Database operations must be sequential because DbContext is not thread-safe
                var kpis = await GetKpisAsync();
                var salesTrend = await GetSalesTrendAsync();

                var payablesTask = GetPayablesAsync();
                var activitiesTask = GetRecentActivitiesAsync();

                await Task.WhenAll(payablesTask, activitiesTask);

                return new DashboardMetricsModel
                {
                    Kpis = kpis,
                    Payables = await payablesTask,
                    Activities = await activitiesTask,
                    SalesTrend = salesTrend
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDashboardMetricsAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Calculates and retrieves Key Performance Indicators (KPIs) for the dashboard.
        /// </summary>
        /// <returns>A model containing various KPI items.</returns>
        public async Task<DashboardKpiModel> GetKpisAsync()
        {
            try
            {
                var todaysSales = await _saleRepository.GetTodaysSalesAsync();
                var totalSalesValue = todaysSales.Sum(s => s.GrandTotalAmount ?? 0);

                var yesterdaysSales = await _saleRepository.GetYesterdaysSalesAsync();
                var yesterdayTotalValue = yesterdaysSales.Sum(s => s.GrandTotalAmount ?? 0);

                string trendString = "+0% from yesterday";
                if (yesterdayTotalValue > 0)
                {
                    var percentageChange = ((totalSalesValue - yesterdayTotalValue) / yesterdayTotalValue) * 100;
                    trendString = $"{(percentageChange >= 0 ? "+" : "")}{percentageChange:F2}% from yesterday";
                }
                else if (totalSalesValue > 0)
                {
                    trendString = "+100% from yesterday";
                }

                var lowStockItems = await _inventoryInteractor.GetLowStockItemsAsync();
                var lowStockList = lowStockItems.ToList();
                
                // Show total count in the header, but only the top 2 most critical alerts in the UI
                var topLowStockAlerts = lowStockList
                    .Take(2)
                    .Select(s => new LowStockAlertModel 
                    { 
                        Name = s.ItemMaster?.vchItemDisplayName ?? "Unknown Item", 
                        Count = (int)(s.numInStock ?? 0) 
                    }).ToList();

                var result = new DashboardKpiModel
                {
                    TotalSales = new DashboardKpiItem
                    {
                        Value = CommonHelper.FormatCurrency(totalSalesValue),
                        Trend = trendString
                    },
                    LowStock = new DashboardKpiLowStockItem
                    {
                        Value = $"{lowStockList.Count:N0} Items",
                        Alerts = topLowStockAlerts
                    },
                    Complaints = new DashboardKpiItem
                    {
                        Value = "05",
                        Trend = "3 pending immediate action"
                    },
                    Balance = new DashboardKpiItem
                    {
                        Value = CommonHelper.FormatCurrency(398220.50m),
                        Trend = "Pending collections"
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetKpisAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the payable metrics for the dashboard.
        /// </summary>
        /// <returns>A model containing total, daily, weekly, and monthly payables.</returns>
        public Task<DashboardPayablesModel> GetPayablesAsync()
        {
            var result = new DashboardPayablesModel
            {
                Total = CommonHelper.FormatCurrency(15082450m),
                Today = CommonHelper.FormatCurrency(12450m),
                Week = CommonHelper.FormatCurrency(82000m),
                Month = CommonHelper.FormatCurrency(245000m)
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// Retrieves a list of recent activities.
        /// </summary>
        /// <param name="topN">The number of activities to retrieve.</param>
        /// <returns>A list of dashboard activity models.</returns>
        public Task<List<DashboardActivityModel>> GetRecentActivitiesAsync(int topN = 10)
        {
            var result = new List<DashboardActivityModel>
            {
                new()
                {
                    Id = "#4402",
                    Name = "James Wilson",
                    Type = "Sale: 2x Wireless Buds",
                    Icon = CommonHelper.ResolveIcon("Sale"),
                    Value = CommonHelper.FormatCurrency(32998.00m),
                    Time = CommonHelper.FormatActivityTime(DateTime.Now.AddHours(-2)),
                    Status = "Completed",
                    StatusVariant = CommonHelper.ResolveStatusVariant("Completed")
                },
                new()
                {
                    Id = "#8812",
                    Name = "Elena Rodriguez",
                    Type = "Complaint: Screen Flicker",
                    Icon = CommonHelper.ResolveIcon("Complaint"),
                    Value = "--",
                    Time = CommonHelper.FormatActivityTime(DateTime.Now.AddHours(-5)),
                    Status = "Pending",
                    StatusVariant = CommonHelper.ResolveStatusVariant("Pending")
                },
                new()
                {
                    Id = "#SUP-10",
                    Name = "TechDistro Inc.",
                    Type = "Purchase: 50x Pro Laptops",
                    Icon = CommonHelper.ResolveIcon("Purchase"),
                    Value = CommonHelper.FormatCurrency(3745000.00m),
                    Time = CommonHelper.FormatActivityTime(DateTime.Now.AddDays(-1)),
                    Status = "In Transit",
                    StatusVariant = CommonHelper.ResolveStatusVariant("In Transit")
                }
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// Retrieves the sales trend data for the last 5 days.
        /// </summary>
        /// <returns>A model containing a list of data points representing the sales trend.</returns>
        public async Task<DashboardSalesTrendModel> GetSalesTrendAsync()
        {
            var result = new DashboardSalesTrendModel();
            var today = DateTime.Today;
            var startDate = today.AddDays(-4);

            // Fetch actual sales from the repository for the 5-day window
            var sales = await _saleRepository.GetSalesByDateRangeAsync(startDate, today.AddDays(1).AddTicks(-1));

            // Generate last 5 days
            for (int i = 4; i >= 0; i--)
            {
                var date = today.AddDays(-i);

                // Sum grand totals for the specific date
                var dayTotal = sales
                    .Where(s => s.SaleDate.HasValue && s.SaleDate.Value.Date == date.Date)
                    .Sum(s => s.GrandTotalAmount ?? 0);

                result.Points.Add(new DashboardSalesTrendPoint
                {
                    Label = date.ToString("dd MMM"),
                    Value = dayTotal
                });
            }

            return result;
        }
        #endregion
    }
}
