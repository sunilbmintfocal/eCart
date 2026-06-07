using System.Collections.Generic;
using System.Threading.Tasks;

namespace MintCart.Api.Domain.Complaint.Interfaces
{
    public interface IComplaintRepository
    {
        Task UpdateCustomerIdAsync(List<int> fromCustomerIds, int toCustomerId);
    }
}
