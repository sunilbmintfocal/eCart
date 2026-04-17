using System;
using System.Collections.Generic;

namespace MintCart.Api.Dashboard.Business.Model
{
    /// <summary>
    /// Raw KPI data returned from the data layer.
    /// </summary>
    public class DashboardKpiData
    {
        public decimal TotalSalesToday { get; set; }
        public decimal TotalSalesYesterday { get; set; }
        public int LowStockItemCount { get; set; }
        public List<LowStockAlert> LowStockAlerts { get; set; } = new();
        public int ActiveComplaintCount { get; set; }
        public int PendingComplaintCount { get; set; }
        public decimal TotalCustomerBalance { get; set; }
    }

    public class LowStockAlert
    {
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Raw payables summary data.
    /// </summary>
    public class DashboardPayablesData
    {
        public decimal TotalOutstanding { get; set; }
        public decimal DueToday { get; set; }
        public decimal DueThisWeek { get; set; }
        public decimal DueThisMonth { get; set; }
    }

    /// <summary>
    /// A single recent activity / transaction row.
    /// </summary>
    public class DashboardActivityData
    {
        public string? ReferenceId { get; set; }
        public string? PartyName { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime ActivityDate { get; set; }
        public string? Status { get; set; }
        public string? ActivityType { get; set; }   // "Sale" | "Complaint" | "Purchase"
    }
}
