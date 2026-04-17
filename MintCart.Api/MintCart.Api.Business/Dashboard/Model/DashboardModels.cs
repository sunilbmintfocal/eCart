namespace MintCart.Api.Dashboard.Business.Model
{
    // ─── KPI Models ───────────────────────────────────────────────────────────

    public class DashboardKpiModel
    {
        public DashboardKpiItem TotalSales { get; set; } = new();
        public DashboardKpiLowStockItem LowStock { get; set; } = new();
        public DashboardKpiItem Complaints { get; set; } = new();
        public DashboardKpiItem Balance { get; set; } = new();
    }

    public class DashboardKpiItem
    {
        public string Value { get; set; } = string.Empty;
        public string Trend { get; set; } = string.Empty;
    }

    public class DashboardKpiLowStockItem
    {
        public string Value { get; set; } = string.Empty;
        public List<LowStockAlertModel> Alerts { get; set; } = new();
    }

    public class LowStockAlertModel
    {
        public string? Name { get; set; }
        public int Count { get; set; }
    }

    // ─── Payables Model ───────────────────────────────────────────────────────

    public class DashboardPayablesModel
    {
        public string Total { get; set; } = string.Empty;
        public string Today { get; set; } = string.Empty;
        public string Week { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
    }

    // ─── Activity Model ───────────────────────────────────────────────────────

    public class DashboardActivityModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Icon { get; set; }
        public string? Value { get; set; }
        public string? Time { get; set; }
        public string? Status { get; set; }
        public string? StatusVariant { get; set; }
    }

    // ─── Aggregated Dashboard Response ────────────────────────────────────────

    public class DashboardMetricsModel
    {
        public DashboardKpiModel Kpis { get; set; } = new();
        public DashboardPayablesModel Payables { get; set; } = new();
        public List<DashboardActivityModel> Activities { get; set; } = new();
    }
}
