using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Customer.Domain.Entities.Customer;
using MintCart.Api.Customer.Domain.Interfaces.Customer;
using MintCart.Api.Domain.Sale.Interfaces;
using MintCart.Api.Domain.Complaint.Interfaces;
using MintCart.Api.Domain.Recharge.Interfaces;

namespace MintCart.Api.Customer.Data.Repository.Customer
{
    /// <summary>
    /// Repository for handling customer persistence operations.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        private readonly ISaleRepository _saleRepository;
        private readonly IComplaintRepository _complaintRepository;
        private readonly IRechargeRepository _rechargeRepository;

        public CustomerRepository(
            CustomerDbContext context,
            ISaleRepository saleRepository,
            IComplaintRepository complaintRepository,
            IRechargeRepository rechargeRepository)
        {
            _context = context;
            _saleRepository = saleRepository;
            _complaintRepository = complaintRepository;
            _rechargeRepository = rechargeRepository;
        }

        #region Public Methods
        /// <summary>
        /// Retrieves all customer entities from the database.
        /// </summary>
        /// <returns>A list of CustomerEntity objects.</returns>
        public async Task<List<CustomerEntity>> GetCustomers()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Paginates in memory rather than via OrderBy().Skip().Take(), since the EF Core
        /// translation for that uses OFFSET/FETCH NEXT, which requires SQL Server
        /// compatibility level 110+. The database's compatibility level (100) does not
        /// support it (see DeleteCustomersAsync for the related OPENJSON issue).
        /// </summary>
        public async Task<(List<CustomerEntity> Items, int TotalCount)> GetCustomersPaged(int page, int pageSize, string? search)
        {
            var query = _context.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    (c.vchCustomerName != null && c.vchCustomerName.Contains(search)) ||
                    (c.vchPhoneNo != null && c.vchPhoneNo.Contains(search)));
            }

            var ordered = await query.OrderByDescending(c => c.dtAddedDate).ToListAsync();
            var items = ordered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return (items, ordered.Count);
        }

        /// <summary>
        /// Returns up to 10 customers whose name or phone contains the query.
        /// Uses Take() which translates to SELECT TOP in SQL Server (compat level 100 safe).
        /// </summary>
        public async Task<List<CustomerEntity>> SuggestCustomers(string q)
        {
            return await _context.Customers
                .AsNoTracking()
                .Where(c =>
                    (c.vchCustomerName != null && c.vchCustomerName.Contains(q)) ||
                    (c.vchPhoneNo != null && c.vchPhoneNo.Contains(q)))
                .OrderBy(c => c.vchCustomerName)
                .Take(10)
                .ToListAsync();
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
        /// <summary>
        /// Merges multiple customers into the primary customer:
        /// 1. Reassigns all FK references (Sale, SaleTransactions, ComplaintReg, Recharge) to the primary ID.
        /// 2. Updates the primary customer's details.
        /// 3. Deactivates all secondary customers.
        /// </summary>
        public async Task<CustomerEntity> MergeCustomers(int primaryCustomerId, List<int> secondaryCustomerIds, CustomerEntity customerDetails)
        {
            await _saleRepository.ReassignCustomerReferencesAsync(secondaryCustomerIds, primaryCustomerId);
            await _complaintRepository.UpdateCustomerIdAsync(secondaryCustomerIds, primaryCustomerId);
            await _rechargeRepository.UpdateCustomerIdAsync(secondaryCustomerIds, primaryCustomerId);

            var primary = await _context.Customers.FirstOrDefaultAsync(x => x.Id == primaryCustomerId);
            if (primary == null)
                throw new InvalidOperationException($"Customer with ID {primaryCustomerId} not found.");

            primary.vchCustomerName = customerDetails.vchCustomerName;
            primary.vchAddress = customerDetails.vchAddress;
            primary.vchShippingAddress = customerDetails.vchShippingAddress;
            primary.vchPhoneNo = customerDetails.vchPhoneNo;
            primary.vchOtherPhoneNo = customerDetails.vchOtherPhoneNo;
            primary.vchIdCardNo = customerDetails.vchIdCardNo;
            primary.vchGSTINNumber = customerDetails.vchGSTINNumber;
            primary.vchState = customerDetails.vchState;
            primary.vchStateCode = customerDetails.vchStateCode;
            primary.vchVCNo = customerDetails.vchVCNo;
            primary.bitIsActive = customerDetails.bitIsActive;
            primary.bitIsBusinessCustomer = customerDetails.bitIsBusinessCustomer;

            await _context.SaveChangesAsync();

            await DeleteCustomersAsync(secondaryCustomerIds);

            return primary;
        }

        /// <summary>
        /// Permanently deletes the given customers via a LINQ-based bulk delete.
        /// Iterates per ID rather than using Contains(), since the database's
        /// compatibility level (100) does not support the OPENJSON translation
        /// EF Core generates for parameterized Contains() queries.
        /// </summary>
        public async Task DeleteCustomersAsync(List<int> customerIds)
        {
            foreach (var id in customerIds)
            {
                await _context.Customers.Where(x => x.Id == id).ExecuteDeleteAsync();
            }
        }
        #endregion
    }
}

