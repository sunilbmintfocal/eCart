namespace MintCart.Api.Business.Reports.Model
{
    public class DailySalesReportModel
    {
        public int SlNo { get; set; }
        public string? Company { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? InvoiceNo { get; set; }
        public string? SaleDateString { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal GSTRate { get; set; }
        public decimal TotalGSTAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
