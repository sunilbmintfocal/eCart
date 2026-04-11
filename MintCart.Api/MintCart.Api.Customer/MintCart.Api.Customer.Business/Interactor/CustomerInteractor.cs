using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MintCart.Api.Customer.Business.Interface;
using MintCart.Api.Customer.Business.Model;
using MintCart.Api.Customer.Domain.Interfaces.Customer;

namespace MintCart.Api.Customer.Business.Interactor
{
    public class CustomerInteractor : ICustomerInteractor
    {
        private readonly ICustomerRepository _repository;

        public CustomerInteractor(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CustomerModel>> GetCustomers()
        {
            var entities = await _repository.GetCustomers();
            return entities.Select(e => new CustomerModel
            {
                Id = Guid.Parse(e.Id),
                FullName = $"{e.FirstName} {e.LastName}",
                EmailAddress = e.Email,
                Mobile = e.PhoneNumber
            }).ToList();
        }
    }
}

