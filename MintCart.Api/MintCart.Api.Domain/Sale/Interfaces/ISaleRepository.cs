using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Domain.Sale.Entities;

namespace MintCart.Api.Domain.Sale.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<SaleEntity>> GetTodaysSalesAsync();
        Task<IEnumerable<SaleEntity>> GetYesterdaysSalesAsync();
        Task<IEnumerable<SaleEntity>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate);
    }
}
