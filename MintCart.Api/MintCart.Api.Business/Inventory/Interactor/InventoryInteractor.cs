using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MintCart.Api.Domain.Inventory.Entities;
using MintCart.Api.Domain.Inventory.Interfaces;
using MintCart.Api.Business.Inventory.Interface;
using Microsoft.Extensions.Configuration;

namespace MintCart.Api.Business.Inventory.Interactor
{
    public class InventoryInteractor : IInventoryInteractor
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IConfiguration _configuration;

        public InventoryInteractor(IInventoryRepository inventoryRepository, IConfiguration configuration)
        {
            _inventoryRepository = inventoryRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ItemStockSummaryEntity>> GetLowStockItemsAsync(decimal threshold = 0)
        {
            try
            {
                if (threshold == 0)
                {
                    threshold = _configuration.GetValue<decimal>("LowStockThreshold", 10);
                }

                var summaries = await _inventoryRepository.GetAllStockSummariesAsync();
                
                // Return items where in-stock quantity is less than or equal to threshold
                return summaries.Where(s => (s.numInStock ?? 0) <= threshold)
                               .OrderBy(s => s.numInStock ?? 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLowStockItemsAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
