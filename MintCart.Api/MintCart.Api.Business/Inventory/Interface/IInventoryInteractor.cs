using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Domain.Inventory.Entities;

namespace MintCart.Api.Business.Inventory.Interface
{
    public interface IInventoryInteractor
    {
        Task<IEnumerable<ItemStockSummaryEntity>> GetLowStockItemsAsync(decimal threshold = 10);
    }
}
