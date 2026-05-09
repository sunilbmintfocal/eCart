using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Domain.Inventory.Entities;

namespace MintCart.Api.Domain.Inventory.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<ItemMasterEntity>> GetAllItemsAsync();
        Task<ItemMasterEntity?> GetItemByIdAsync(int id);
        Task<ItemMasterEntity> CreateItemAsync(ItemMasterEntity item);
        Task UpdateItemAsync(ItemMasterEntity item);
        
        Task<IEnumerable<ItemStockEntity>> GetStockByItemIdAsync(int itemId);
        Task<ItemStockSummaryEntity?> GetStockSummaryAsync(int itemId);
        Task<IEnumerable<ItemStockSummaryEntity>> GetAllStockSummariesAsync();
        Task AddStockTransactionAsync(ItemStockTransEntity trans);
    }
}
