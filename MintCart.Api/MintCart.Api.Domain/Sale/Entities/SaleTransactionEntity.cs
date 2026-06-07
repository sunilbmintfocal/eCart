using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Sale.Entities
{
    [Table("SaleTransactions", Schema = "dbo")]
    public class SaleTransactionEntity
    {
        [Key]
        [Column("intSaleTransactionId")]
        public int Id { get; set; }

        [Column("intSaleId")]
        public int? SaleId { get; set; }

        [Column("intCustomerId")]
        public int? CustomerId { get; set; }

        [Column("numSaleAmount")]
        public decimal? SaleAmount { get; set; }

        [Column("numSaleAmountPaid")]
        public decimal? SaleAmountPaid { get; set; }

        [Column("bitStatus")]
        public bool? Status { get; set; }

        [Column("dtSaleDate")]
        public DateTime? SaleDate { get; set; }

        [Column("intUserId")]
        public int? UserId { get; set; }
    }
}
