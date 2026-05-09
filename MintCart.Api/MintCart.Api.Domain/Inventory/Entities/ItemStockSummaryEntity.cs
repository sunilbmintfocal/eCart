using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Inventory.Entities
{
    [Table("ItemStockSummary", Schema = "dbo")]
    public class ItemStockSummaryEntity
    {
        [Key]
        [Column("intItemStockSummaryId")]
        public int Id { get; set; }

        [Column("intItemMasterId")]
        public int? ItemMasterId { get; set; }

        public string? vchBatchNumber { get; set; }
        public decimal? numStock { get; set; }
        public decimal? numStockReturn { get; set; }
        public decimal? numTotalStok { get; set; }
        public decimal? numSale { get; set; }
        public decimal? numSalesReturn { get; set; }
        public decimal? numTotalSale { get; set; }
        public decimal? numInStock { get; set; }
        public DateTime? dtLastUpdatedDate { get; set; }
        public int? intUserId { get; set; }

        [ForeignKey("ItemMasterId")]
        public virtual ItemMasterEntity? ItemMaster { get; set; }
    }
}
