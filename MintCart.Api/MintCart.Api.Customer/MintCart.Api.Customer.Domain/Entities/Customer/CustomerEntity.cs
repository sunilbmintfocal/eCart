using System;
using MintCart.Api.Customer.Domain.Entities.Base;

namespace MintCart.Api.Customer.Domain.Entities.Customer
{
    public class CustomerEntity : BaseEntityAudit<string>
    {
        public CustomerEntity()
        {
            Id = Guid.NewGuid().ToString();
            IsDeleted = false;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

