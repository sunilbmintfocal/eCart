using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Sale.Entities
{
    [Table("Sale", Schema = "dbo")]
    public class SaleEntity
    {
        [Key]
        [Column("intSaleId")]
        public int Id { get; set; }

        [Column("intBillingCustomerId")]
        public int? BillingCustomerId { get; set; }

        [Column("vchShippingAddress")]
        public string? ShippingAddress { get; set; }

        [Column("intUserId")]
        public int? UserId { get; set; }

        [Column("vchInvoiceNumber")]
        public string? InvoiceNumber { get; set; }

        [Column("dtSaleDate")]
        public DateTime? SaleDate { get; set; }

        [Column("numTotalAmount")]
        public decimal? TotalAmount { get; set; }

        [Column("numTotalDiscount")]
        public decimal? TotalDiscount { get; set; }

        [Column("numTotalTaxableAmount")]
        public decimal? TotalTaxableAmount { get; set; }

        [Column("numTotalCGST")]
        public decimal? TotalCGST { get; set; }

        [Column("numTotalSGST")]
        public decimal? TotalSGST { get; set; }

        [Column("numTotalIGST")]
        public decimal? TotalIGST { get; set; }

        [Column("numTotalGST")]
        public decimal? TotalGST { get; set; }

        [Column("numGrandTotalAmount")]
        public decimal? GrandTotalAmount { get; set; }

        [Column("bitIsCancelled")]
        public bool? IsCancelled { get; set; }
    }
}
