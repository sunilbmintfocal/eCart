using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Customer.Domain.Entities.Customer;

namespace MintCart.Api.Customer.Domain.Interfaces.Customer
{
    public interface ICustomerRepository
    {
        Task<List<CustomerEntity>> GetCustomers();
        Task<(List<CustomerEntity> Items, int TotalCount)> GetCustomersPaged(int page, int pageSize, string? search);
        Task<CustomerEntity> UpsertCustomer(CustomerEntity customer);
        Task<CustomerEntity> MergeCustomers(int primaryCustomerId, List<int> secondaryCustomerIds, CustomerEntity customerDetails);
        Task DeleteCustomersAsync(List<int> customerIds);
    }
}

