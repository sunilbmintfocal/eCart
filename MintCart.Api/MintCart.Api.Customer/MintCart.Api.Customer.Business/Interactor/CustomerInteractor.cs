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
    public class CustomerInteractor : ICustomerInteractor
    {
        private readonly ICustomerRepository _repository;

        public CustomerInteractor(ICustomerRepository repository)
        {
            _repository = repository;
        }

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
                dtAddedDate = customer.AddedDate,
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
    }
}

