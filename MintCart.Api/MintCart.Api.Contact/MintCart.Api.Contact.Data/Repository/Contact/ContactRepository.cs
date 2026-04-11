using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Contact.Domain.Entities.Contact;
using MintCart.Api.Contact.Domain.Interfaces.Contact;

namespace MintCart.Api.Contact.Data.Repository.Contact
{
    public class ContactRepository : IContactRepository
    {
        public async Task<List<ContactEntity>> GetContacts()
        {
            return await Task.FromResult(new List<ContactEntity>
            {
                new ContactEntity 
                { 
                    FirstName = "Test", 
                    LastName = "User", 
                    Email = "testuser@mintcart.com", 
                    PhoneNumber = "1234567890",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = "System",
                    ModifiedDate = DateTime.UtcNow
                }
            });
        }
    }
}
