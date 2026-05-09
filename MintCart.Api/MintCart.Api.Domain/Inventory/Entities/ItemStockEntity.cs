using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Inventory.Entities
{
    [Table("ItemStock", Schema = "dbo")]
    public class ItemStockEntity
    {
        [Key]
        [Column("intItemStockId")]
        public int Id { get; set; }

        [Column("intItemMasterId")]
        public int? ItemMasterId { get; set; }

        public string? vchBatchNumber { get; set; }
        public decimal? numStockQty { get; set; }
        public decimal? numStockRetQty { get; set; }
        public decimal? numTotalStockQty { get; set; }
        
        public decimal? numPurchase_Amount { get; set; }
        public decimal? numPurchase_WithoutGST_Amount { get; set; }
        public decimal? numPurchase_GST_Amount { get; set; }
        
        public decimal? numPurchase_Audit_Amount { get; set; }
        public decimal? numPurchase_Audit_WithoutGST_Amount { get; set; }
        public decimal? numPurchase_Audit_GST_Amount { get; set; }
        
        public decimal? numSelling_Amount { get; set; }
        public decimal? numSelling_WithoutGST_Amount { get; set; }
        public decimal? numSelling_GST_Amount { get; set; }
        
        public decimal? numSelling_Audit_Amount { get; set; }
        public decimal? numSelling_Audit_WithoutGST_Amount { get; set; }
        public decimal? numSelling_Audit_GST_Amount { get; set; }
        
        public DateTime? dtStockDate { get; set; }
        public int? intAddedUserId { get; set; }
        
        public bool? bitIsPurchaseAudit { get; set; }
        public bool? bitIsSellingAudit { get; set; }
        public bool? bitIsTaxItem { get; set; }
        public bool? bitOldGst { get; set; }

        [ForeignKey("ItemMasterId")]
        public virtual ItemMasterEntity? ItemMaster { get; set; }
    }
}
