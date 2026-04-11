using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Customer.Business.Model;

namespace MintCart.Api.Customer.Business.Interface
{
    public interface ICustomerInteractor
    {
        Task<List<CustomerModel>> GetCustomers();
    }
}

