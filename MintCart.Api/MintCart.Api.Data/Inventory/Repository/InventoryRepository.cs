using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Domain.Inventory.Entities;
using MintCart.Api.Domain.Inventory.Interfaces;

namespace MintCart.Api.Data.Inventory.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemMasterEntity>> GetAllItemsAsync()
        {
            return await _context.ItemMasters.AsNoTracking().ToListAsync();
        }

        public async Task<ItemMasterEntity?> GetItemByIdAsync(int id)
        {
            return await _context.ItemMasters.FindAsync(id);
        }

        public async Task<ItemMasterEntity> CreateItemAsync(ItemMasterEntity item)
        {
            _context.ItemMasters.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateItemAsync(ItemMasterEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ItemStockEntity>> GetStockByItemIdAsync(int itemId)
        {
            return await _context.ItemStocks.AsNoTracking().Where(s => s.ItemMasterId == itemId).ToListAsync();
        }

        public async Task<ItemStockSummaryEntity?> GetStockSummaryAsync(int itemId)
        {
            return await _context.ItemStockSummaries.AsNoTracking().FirstOrDefaultAsync(s => s.ItemMasterId == itemId);
        }

        public async Task<IEnumerable<ItemStockSummaryEntity>> GetAllStockSummariesAsync()
        {
            return await _context.ItemStockSummaries.AsNoTracking().Include(s => s.ItemMaster).ToListAsync();
        }

        public async Task AddStockTransactionAsync(ItemStockTransEntity trans)
        {
            _context.ItemStockTransactions.Add(trans);
            
            var summary = await _context.ItemStockSummaries.FirstOrDefaultAsync(s => s.ItemMasterId == trans.ItemMasterId);
            if (summary == null)
            {
                summary = new ItemStockSummaryEntity 
                { 
                    ItemMasterId = trans.ItemMasterId,
                    vchBatchNumber = trans.vchBatchNumber,
                    numStock = trans.vchTransaction == "IN" ? (trans.numQtyTrans ?? 0) : 0,
                    numSale = trans.vchTransaction == "OUT" ? (trans.numQtyTrans ?? 0) : 0,
                    numInStock = trans.vchTransaction == "IN" ? (trans.numQtyTrans ?? 0) : -(trans.numQtyTrans ?? 0)
                };
                _context.ItemStockSummaries.Add(summary);
            }
            else
            {
                if (trans.vchTransaction == "IN")
                {
                    summary.numStock = (summary.numStock ?? 0) + (trans.numQtyTrans ?? 0);
                    summary.numInStock = (summary.numInStock ?? 0) + (trans.numQtyTrans ?? 0);
                }
                else if (trans.vchTransaction == "OUT")
                {
                    summary.numSale = (summary.numSale ?? 0) + (trans.numQtyTrans ?? 0);
                    summary.numInStock = (summary.numInStock ?? 0) - (trans.numQtyTrans ?? 0);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
