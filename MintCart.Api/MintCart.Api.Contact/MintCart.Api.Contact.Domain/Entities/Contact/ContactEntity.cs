using System;
using MintCart.Api.Contact.Domain.Entities.Base;

namespace MintCart.Api.Contact.Domain.Entities.Contact
{
    public class ContactEntity : BaseEntityAudit<string>
    {
        public ContactEntity()
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
