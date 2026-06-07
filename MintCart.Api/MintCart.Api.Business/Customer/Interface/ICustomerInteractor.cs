using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Customer.Business.Model;

namespace MintCart.Api.Customer.Business.Interface
{
    public interface ICustomerInteractor
    {
        Task<List<CustomerModel>> GetCustomers();
        Task<PagedResult<CustomerModel>> GetCustomersPaged(int page, int pageSize, string? search);
        Task<CustomerModel> UpsertCustomer(CustomerModel customer);
        Task<CustomerModel> MergeCustomers(MergeCustomerModel mergeRequest);
    }
}

