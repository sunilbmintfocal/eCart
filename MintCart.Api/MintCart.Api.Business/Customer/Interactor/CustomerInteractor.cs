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
            return entities.Select(MapToModel).ToList();
        }

        public async Task<PagedResult<CustomerModel>> GetCustomersPaged(int page, int pageSize, string? search)
        {
            var (entities, total) = await _repository.GetCustomersPaged(page, pageSize, search);
            return new PagedResult<CustomerModel>
            {
                Items = entities.Select(MapToModel).ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<List<CustomerSuggestionModel>> SuggestCustomers(string q)
        {
            var entities = await _repository.SuggestCustomers(q);
            return entities.Select(e => new CustomerSuggestionModel
            {
                Id = e.Id,
                CustomerName = e.vchCustomerName,
                PhoneNo = e.vchPhoneNo
            }).ToList();
        }

        private static CustomerModel MapToModel(CustomerEntity e) => new CustomerModel
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
        };

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
        /// <summary>
        /// Merges multiple customer profiles into the primary customer.
        /// Updates the primary customer's details with the provided information.
        /// </summary>
        /// <param name="mergeRequest">The merge request containing customer IDs and new details.</param>
        /// <returns>The updated primary <see cref="CustomerModel"/>.</returns>
        public async Task<CustomerModel> MergeCustomers(MergeCustomerModel mergeRequest)
        {
            var primaryCustomerId = mergeRequest.CustomerIds[0];
            var secondaryCustomerIds = mergeRequest.CustomerIds.Skip(1).ToList();
            var details = mergeRequest.CustomerDetails;

            var entity = new CustomerEntity
            {
                vchCustomerName = details.CustomerName,
                vchAddress = details.Address,
                vchShippingAddress = details.ShippingAddress,
                vchPhoneNo = details.PhoneNo,
                vchOtherPhoneNo = details.OtherPhoneNo,
                vchIdCardNo = details.IdCardNo,
                vchGSTINNumber = details.GSTINNumber,
                vchState = details.State,
                vchStateCode = details.StateCode,
                vchVCNo = details.VCNo,
                bitIsActive = details.IsActive,
                bitIsBusinessCustomer = details.IsBusinessCustomer
            };

            var result = await _repository.MergeCustomers(primaryCustomerId, secondaryCustomerIds, entity);

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

