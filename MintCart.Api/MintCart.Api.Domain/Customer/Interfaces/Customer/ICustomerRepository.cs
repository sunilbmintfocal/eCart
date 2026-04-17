using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Customer.Domain.Entities.Customer;

namespace MintCart.Api.Customer.Domain.Interfaces.Customer
{
    public interface ICustomerRepository
    {
        Task<List<CustomerEntity>> GetCustomers();
        Task<CustomerEntity> UpsertCustomer(CustomerEntity customer);
    }
}

