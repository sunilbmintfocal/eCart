using System;
using System.ComponentModel.DataAnnotations.Schema;
using MintCart.Api.Customer.Domain.Entities.Base;

namespace MintCart.Api.Customer.Domain.Entities.Customer
{
    [Table("Customer", Schema = "dbo")]
    public class CustomerEntity 
    {
        public CustomerEntity()
        {
            bitIsActive = true;
        }

        [Column("intCustomerId")]
        public int Id { get; set; }

        public string? vchCustomerName { get; set; }
        public string? vchAddress { get; set; }
        public string? vchShippingAddress { get; set; }
        public string? vchPhoneNo { get; set; }
        public string? vchOtherPhoneNo { get; set; }
        public string? vchIdCardNo { get; set; }
        public DateTime? dtAddedDate { get; set; }
        public bool? bitIsActive { get; set; }
        public string? vchGSTINNumber { get; set; }
        public bool? bitIsBusinessCustomer { get; set; }
        public string? vchState { get; set; }
        public string? vchStateCode { get; set; }
        public string? vchVCNo { get; set; }
    }
}
