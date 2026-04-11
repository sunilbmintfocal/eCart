using System;

namespace MintCart.Api.Customer.Business.Model
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
    }
}

