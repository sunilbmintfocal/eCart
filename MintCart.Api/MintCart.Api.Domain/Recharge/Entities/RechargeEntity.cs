using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Recharge.Entities
{
    [Table("Recharge", Schema = "dbo")]
    public class RechargeEntity
    {
        [Key]
        [Column("intRechargeId")]
        public int Id { get; set; }

        [Column("intOperatorId")]
        public int? OperatorId { get; set; }

        [Column("vchBasicPack")]
        public string? BasicPack { get; set; }

        [Column("numAmount")]
        public decimal? Amount { get; set; }

        [Column("vchAddOnPack")]
        public string? AddOnPack { get; set; }

        [Column("numAddOnPackAmt")]
        public decimal? AddOnPackAmount { get; set; }

        [Column("numTotalAmt")]
        public decimal? TotalAmount { get; set; }

        [Column("intCustomerId")]
        public int? CustomerId { get; set; }

        [Column("vchRefNo")]
        public string? RefNo { get; set; }

        [Column("dtDate")]
        public DateTime? Date { get; set; }

        [Column("vchType")]
        public string? Type { get; set; }

        [Column("dtUpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("bitStatus")]
        public bool? Status { get; set; }

        [Column("bitIsPaid")]
        public bool? IsPaid { get; set; }
    }
}
