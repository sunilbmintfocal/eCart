using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Sale.Entities
{
    [Table("SaleChild", Schema = "dbo")]
    public class SaleChildEntity
    {
        [Key]
        [Column("intSaleChildId")]
        public int Id { get; set; }

        [Column("intSaleId")]
        public int? SaleId { get; set; }

        [Column("intItemMasterId")]
        public int? ItemMasterId { get; set; }

        [Column("numQuantity")]
        public decimal? Quantity { get; set; }

        [Column("numActual_Selling_Rate")]
        public decimal? ActualSellingRate { get; set; }

        [Column("numActual_Selling_Amount")]
        public decimal? ActualSellingAmount { get; set; }

        [Column("numDiscount_Amount")]
        public decimal? DiscountAmount { get; set; }

        [Column("numTaxableAmount")]
        public decimal? TaxableAmount { get; set; }

        [Column("numTotalGSTAmount")]
        public decimal? TotalGSTAmount { get; set; }

        [Column("numTotalAmount")]
        public decimal? TotalAmount { get; set; }

        [Column("dtSaleDate")]
        public DateTime? SaleDate { get; set; }
    }
}
