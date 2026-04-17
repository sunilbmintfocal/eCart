using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Customer.Domain.Entities.Customer;
using MintCart.Api.Customer.Domain.Interfaces.Customer;

namespace MintCart.Api.Customer.Data.Repository.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerEntity>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }
        
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
    }
}

