using MintCart.Api.Dashboard.Business.Interface;
using MintCart.Api.Dashboard.Business.Model;
using MintCart.Api.Domain.Sale.Interfaces;
using System.Linq;

namespace MintCart.Api.Dashboard.Business.Interactor
{
    public class DashboardInteractor : IDashboardInteractor
    {
        private readonly ISaleRepository _saleRepository;

        public DashboardInteractor(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Aggregated single-call endpoint
        // ─────────────────────────────────────────────────────────────────────
        public async Task<DashboardMetricsModel> GetDashboardMetricsAsync()
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

        // ─────────────────────────────────────────────────────────────────────
        // KPIs
        // ─────────────────────────────────────────────────────────────────────
        public async Task<DashboardKpiModel> GetKpisAsync()
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

            var result = new DashboardKpiModel
            {
                TotalSales = new DashboardKpiItem
                {
                    Value = FormatCurrency(totalSalesValue),
                    Trend = trendString
                },
                LowStock = new DashboardKpiLowStockItem
                {
                    Value  = "08 Items",
                    Alerts = new List<LowStockAlertModel>
                    {
                        new() { Name = "Pro Laptops", Count = 2 },
                        new() { Name = "Smartphones", Count = 6 }
                    }
                },
                Complaints = new DashboardKpiItem
                {
                    Value = "05",
                    Trend = "3 pending immediate action"
                },
                Balance = new DashboardKpiItem
                {
                    Value = FormatCurrency(398220.50m),
                    Trend = "Pending collections"
                }
            };

            return result;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Payables
        // ─────────────────────────────────────────────────────────────────────
        public Task<DashboardPayablesModel> GetPayablesAsync()
        {
            var result = new DashboardPayablesModel
            {
                Total = FormatCurrency(15082450m),
                Today = FormatCurrency(12450m),
                Week  = FormatCurrency(82000m),
                Month = FormatCurrency(245000m)
            };

            return Task.FromResult(result);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Recent Activities
        // ─────────────────────────────────────────────────────────────────────
        public Task<List<DashboardActivityModel>> GetRecentActivitiesAsync(int topN = 10)
        {
            var result = new List<DashboardActivityModel>
            {
                new()
                {
                    Id            = "#4402",
                    Name          = "James Wilson",
                    Type          = "Sale: 2x Wireless Buds",
                    Icon          = ResolveIcon("Sale"),
                    Value         = FormatCurrency(32998.00m),
                    Time          = FormatActivityTime(DateTime.Now.AddHours(-2)),
                    Status        = "Completed",
                    StatusVariant = ResolveStatusVariant("Completed")
                },
                new()
                {
                    Id            = "#8812",
                    Name          = "Elena Rodriguez",
                    Type          = "Complaint: Screen Flicker",
                    Icon          = ResolveIcon("Complaint"),
                    Value         = "--",
                    Time          = FormatActivityTime(DateTime.Now.AddHours(-5)),
                    Status        = "Pending",
                    StatusVariant = ResolveStatusVariant("Pending")
                },
                new()
                {
                    Id            = "#SUP-10",
                    Name          = "TechDistro Inc.",
                    Type          = "Purchase: 50x Pro Laptops",
                    Icon          = ResolveIcon("Purchase"),
                    Value         = FormatCurrency(3745000.00m),
                    Time          = FormatActivityTime(DateTime.Now.AddDays(-1)),
                    Status        = "In Transit",
                    StatusVariant = ResolveStatusVariant("In Transit")
                }
            };
            
            return Task.FromResult(result);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────────────
        private static string FormatCurrency(decimal amount)
        {
            // Indian number format with ₹ symbol
            return $"₹{amount:##,##,##0.00}";
        }

        private static string FormatActivityTime(DateTime date)
        {
            var today     = DateTime.Today;
            var yesterday = today.AddDays(-1);

            if (date.Date == today)
                return $"Today, {date:hh:mm tt}";

            if (date.Date == yesterday)
                return $"Yesterday, {date:hh:mm tt}";

            return date.ToString("dd MMM yyyy, hh:mm tt");
        }

        private static string ResolveIcon(string? activityType) => activityType switch
        {
            "Sale"      => "shopping_bag",
            "Complaint" => "assignment_late",
            "Purchase"  => "local_shipping",
            _           => "receipt_long"
        };

        private static string ResolveStatusVariant(string? status) => status?.ToLower() switch
        {
            "completed"  => "primary",
            "pending"    => "error",
            "in transit" => "secondary",
            "cancelled"  => "error",
            _            => "secondary"
        };

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
    }
}
