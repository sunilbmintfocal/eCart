using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Customer.Domain.Entities.Customer;
using MintCart.Api.Customer.Domain.Interfaces.Customer;

namespace MintCart.Api.Customer.Data.Repository.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task<List<CustomerEntity>> GetCustomers()
        {
            return await Task.FromResult(new List<CustomerEntity>
            {
                new CustomerEntity 
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

