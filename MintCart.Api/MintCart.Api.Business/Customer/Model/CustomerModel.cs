using System;

namespace MintCart.Api.Customer.Business.Model
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PhoneNo { get; set; }
        public string? OtherPhoneNo { get; set; }
        public string? IdCardNo { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool? IsActive { get; set; }
        public string? GSTINNumber { get; set; }
        public bool? IsBusinessCustomer { get; set; }
        public string? State { get; set; }
        public string? StateCode { get; set; }
        public string? VCNo { get; set; }
    }
}

