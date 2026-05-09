using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Inventory.Entities
{
    [Table("ItemStockTrans", Schema = "dbo")]
    public class ItemStockTransEntity
    {
        [Key]
        [Column("intItemStockTransId")]
        public int Id { get; set; }

        [Column("intItemMasterId")]
        public int? ItemMasterId { get; set; }

        public string? vchBatchNumber { get; set; }
        public decimal? numQtyTrans { get; set; }
        public string? vchTransaction { get; set; }
        public string? vchDescription { get; set; }
        public DateTime? dtDate { get; set; }
        public int? intRefferenceId { get; set; }
        public int? intUserId { get; set; }

        [ForeignKey("ItemMasterId")]
        public virtual ItemMasterEntity? ItemMaster { get; set; }
    }
}
