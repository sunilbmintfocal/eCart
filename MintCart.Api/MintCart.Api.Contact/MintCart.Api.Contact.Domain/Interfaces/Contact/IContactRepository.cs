using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Contact.Domain.Entities.Contact;

namespace MintCart.Api.Contact.Domain.Interfaces.Contact
{
    public interface IContactRepository
    {
        Task<List<ContactEntity>> GetContacts();
    }
}
