using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MintCart.Api.Customer.Business.Interface;
using MintCart.Api.Customer.Business.Model;
using MintCart.Api.Customer.Domain.Entities.Customer;
using MintCart.Api.Customer.Domain.Interfaces.Customer;

namespace MintCart.Api.Customer.Business.Interactor
{
    /// <summary>
    /// Handles the business logic for customer management.
    /// </summary>
    public class CustomerInteractor : ICustomerInteractor
    {
        private readonly ICustomerRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerInteractor"/> class.
        /// </summary>
        /// <param name="repository">The customer repository.</param>
        public CustomerInteractor(ICustomerRepository repository)
        {
            _repository = repository;
        }

        #region Public Methods
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns>A list of <see cref="CustomerModel"/>.</returns>
        public async Task<List<CustomerModel>> GetCustomers()
        {
            var entities = await _repository.GetCustomers();
            return entities.Select(e => new CustomerModel
            {
                Id = e.Id,
                CustomerName = e.vchCustomerName,
                Address = e.vchAddress,
                ShippingAddress = e.vchShippingAddress,
                PhoneNo = e.vchPhoneNo,
                OtherPhoneNo = e.vchOtherPhoneNo,
                IdCardNo = e.vchIdCardNo,
                AddedDate = e.dtAddedDate,
                IsActive = e.bitIsActive,
                GSTINNumber = e.vchGSTINNumber,
                IsBusinessCustomer = e.bitIsBusinessCustomer,
                State = e.vchState,
                StateCode = e.vchStateCode,
                VCNo = e.vchVCNo
            }).ToList();
        }

        /// <summary>
        /// Creates or updates a customer.
        /// </summary>
        /// <param name="customer">The customer data to upsert.</param>
        /// <returns>The upserted <see cref="CustomerModel"/>.</returns>
        public async Task<CustomerModel> UpsertCustomer(CustomerModel customer)
        {
            var entity = new CustomerEntity
            {
                Id = customer.Id,
                vchCustomerName = customer.CustomerName,
                vchAddress = customer.Address,
                vchShippingAddress = customer.ShippingAddress,
                vchPhoneNo = customer.PhoneNo,
                vchOtherPhoneNo = customer.OtherPhoneNo,
                vchIdCardNo = customer.IdCardNo,
                dtAddedDate = DateTime.Now,
                bitIsActive = customer.IsActive,
                vchGSTINNumber = customer.GSTINNumber,
                bitIsBusinessCustomer = customer.IsBusinessCustomer,
                vchState = customer.State,
                vchStateCode = customer.StateCode,
                vchVCNo = customer.VCNo
            };

            var result = await _repository.UpsertCustomer(entity);

            return new CustomerModel
            {
                Id = result.Id,
                CustomerName = result.vchCustomerName,
                Address = result.vchAddress,
                ShippingAddress = result.vchShippingAddress,
                PhoneNo = result.vchPhoneNo,
                OtherPhoneNo = result.vchOtherPhoneNo,
                IdCardNo = result.vchIdCardNo,
                AddedDate = result.dtAddedDate,
                IsActive = result.bitIsActive,
                GSTINNumber = result.vchGSTINNumber,
                IsBusinessCustomer = result.bitIsBusinessCustomer,
                State = result.vchState,
                StateCode = result.vchStateCode,
                VCNo = result.vchVCNo
            };
        }
        #endregion
    }
}

