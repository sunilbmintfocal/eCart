using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Customer.Business.Interface;
using MintCart.Api.Customer.Business.Model;
using MintCart.Api.Core.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MintCart.Api.Customer.API.Controllers
{
    [Route("api/customer")]
    public class CustomerController : BaseController<CustomerController>
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerInteractor _customerInteractor;

        public CustomerController(ILogger<CustomerController> logger, ICustomerInteractor customerInteractor) : base(logger)
        {
            _logger = logger;
            _customerInteractor = customerInteractor;
        }

        /// <summary>
        /// Gives all the Customers as a list of objects
        /// </summary>
        /// <returns>Returns the list of Customers</returns>
        [HttpGet]
        [Route("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var res = await _customerInteractor.GetCustomers();
            return await CreateApiResponse(res);
        }
    }
}

