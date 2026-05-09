using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Inventory.Entities
{
    [Table("ItemMaster", Schema = "dbo")]
    public class ItemMasterEntity
    {
        [Key]
        [Column("intItemMasterId")]
        public int Id { get; set; }

        public string? vchItemCode { get; set; }
        public string? vchItemDescription { get; set; }
        public string? vchItemDisplayName { get; set; }
        public int? intCategoryId { get; set; }
        public int? intUMId { get; set; }
        public int? intHSNCodeId { get; set; }
        public string? vchModelNumber { get; set; }
        public string? vchBrandName { get; set; }
        public string? vchSize { get; set; }
        public string? vchColor { get; set; }
        public string? vchSupplier { get; set; }
        public string? vchWarranty { get; set; }
        public string? vchYearOfManufacture { get; set; }
        public string? vchCountry { get; set; }
        public DateTime? dtAddedDate { get; set; }
        public int? intUserId { get; set; }
    }
}
