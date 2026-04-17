using MintCart.Api.Dashboard.Business.Interface;
using MintCart.Api.Dashboard.Business.Model;

namespace MintCart.Api.Dashboard.Business.Interactor
{
    public class DashboardInteractor : IDashboardInteractor
    {
        public DashboardInteractor()
        {
        }

        // ─────────────────────────────────────────────────────────────────────
        // Aggregated single-call endpoint
        // ─────────────────────────────────────────────────────────────────────
        public async Task<DashboardMetricsModel> GetDashboardMetricsAsync()
        {
            var kpisTask       = GetKpisAsync();
            var payablesTask   = GetPayablesAsync();
            var activitiesTask = GetRecentActivitiesAsync();

            await Task.WhenAll(kpisTask, payablesTask, activitiesTask);

            return new DashboardMetricsModel
            {
                Kpis       = await kpisTask,
                Payables   = await payablesTask,
                Activities = await activitiesTask
            };
        }

        // ─────────────────────────────────────────────────────────────────────
        // KPIs
        // ─────────────────────────────────────────────────────────────────────
        public Task<DashboardKpiModel> GetKpisAsync()
        {
            var result = new DashboardKpiModel
            {
                TotalSales = new DashboardKpiItem
                {
                    Value = FormatCurrency(102482.00m),
                    Trend = "+11.99% from yesterday"
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

            return Task.FromResult(result);
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
    }
}
