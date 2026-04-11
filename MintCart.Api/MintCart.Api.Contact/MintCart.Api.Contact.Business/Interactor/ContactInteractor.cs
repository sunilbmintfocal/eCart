using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MintCart.Api.Contact.Business.Interface;
using MintCart.Api.Contact.Business.Model;
using MintCart.Api.Contact.Domain.Interfaces.Contact;

namespace MintCart.Api.Contact.Business.Interactor
{
    public class ContactInteractor : IContactInteractor
    {
        private readonly IContactRepository _repository;

        public ContactInteractor(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContactModel>> GetContacts()
        {
            var entities = await _repository.GetContacts();
            return entities.Select(e => new ContactModel
            {
                Id = Guid.Parse(e.Id),
                FullName = $"{e.FirstName} {e.LastName}",
                EmailAddress = e.Email,
                Mobile = e.PhoneNumber
            }).ToList();
        }
    }
}
