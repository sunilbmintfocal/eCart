using System;
using System.Threading.Tasks;

namespace MintCart.Api.Contact.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChanges();
    }
}
