using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Customer.Domain.Entities.Customer;
using MintCart.Api.Customer.Domain.Interfaces.Customer;

namespace MintCart.Api.Customer.Data.Repository.Customer
{
    /// <summary>
    /// Repository for handling customer persistence operations.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The customer database context.</param>
        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        #region Public Methods
        /// <summary>
        /// Retrieves all customer entities from the database.
        /// </summary>
        /// <returns>A list of CustomerEntity objects.</returns>
        public async Task<List<CustomerEntity>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Upserts a customer entity. Creates a new record if Id is 0, otherwise updates existing.
        /// </summary>
        /// <param name="customer">The customer entity to save.</param>
        /// <returns>The saved CustomerEntity.</returns>
        public async Task<CustomerEntity> UpsertCustomer(CustomerEntity customer)
        {
            if (customer.Id <= 0)
            {
                customer.dtAddedDate = DateTime.Now;
                _context.Customers.Add(customer);
            }
            else
            {
                var existing = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == customer.Id);
                if (existing != null)
                {
                    customer.dtAddedDate = DateTime.Now;
                }
                _context.Customers.Update(customer);
            }
            await _context.SaveChangesAsync();
            return customer;
        }
        #endregion
    }
}

