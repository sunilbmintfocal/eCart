using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Contact.Business.Model;

namespace MintCart.Api.Contact.Business.Interface
{
    public interface IContactInteractor
    {
        Task<List<ContactModel>> GetContacts();
    }
}
