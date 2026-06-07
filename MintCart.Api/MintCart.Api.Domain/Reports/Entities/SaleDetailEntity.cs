namespace MintCart.Api.Domain.Reports.Entities
{
    /// <summary>
    /// Keyless entity mapping the result set returned by [sprptSaleDetails1].
    /// Property names must match the column names output by the stored procedure exactly.
    /// </summary>
    public class SaleDetailEntity
    {
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
