using System;
using System.Threading.Tasks;

namespace MintCart.Api.Customer.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChanges();
    }
}

