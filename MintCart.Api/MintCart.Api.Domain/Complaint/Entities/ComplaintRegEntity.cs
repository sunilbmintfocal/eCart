using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Api.Domain.Complaint.Entities
{
    [Table("ComplaintReg", Schema = "dbo")]
    public class ComplaintRegEntity
    {
        [Key]
        [Column("intComplaintId")]
        public int Id { get; set; }

        [Column("intSaleId")]
        public int? SaleId { get; set; }

        [Column("intSaleChildId")]
        public int? SaleChildId { get; set; }

        [Column("intCustomerId")]
        public int? CustomerId { get; set; }

        [Column("dtRegDate")]
        public DateTime? RegDate { get; set; }

        [Column("vchComplaintNo")]
        public string? ComplaintNo { get; set; }

        [Column("dtLastDateofSolve")]
        public DateTime? LastDateOfSolve { get; set; }

        [Column("vchStatus")]
        public string? Status { get; set; }

        [Column("vchComplaintDescription")]
        public string? ComplaintDescription { get; set; }

        [Column("vchComplaitTo")]
        public string? ComplaintTo { get; set; }

        [Column("vchComplaitSolution")]
        public string? ComplaintSolution { get; set; }

        [Column("dtFixedDate")]
        public DateTime? FixedDate { get; set; }

        [Column("vchType")]
        public string? Type { get; set; }

        [Column("vchRefComplaintNo")]
        public string? RefComplaintNo { get; set; }

        [Column("vchPrdctSlNo")]
        public string? ProductSerialNo { get; set; }

        [Column("numAmtChrgd")]
        public decimal? AmountCharged { get; set; }

        [Column("dtRetDate")]
        public DateTime? ReturnDate { get; set; }
    }
}
