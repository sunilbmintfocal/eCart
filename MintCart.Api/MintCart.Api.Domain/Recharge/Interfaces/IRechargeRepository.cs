using System.Collections.Generic;
using System.Threading.Tasks;

namespace MintCart.Api.Domain.Recharge.Interfaces
{
    public interface IRechargeRepository
    {
        Task UpdateCustomerIdAsync(List<int> fromCustomerIds, int toCustomerId);
    }
}
