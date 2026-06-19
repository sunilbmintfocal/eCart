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
    /// <summary>
    /// API controller for customer-related operations.
    /// </summary>
    [Route("api/customer")]
    public class CustomerController : BaseController<CustomerController>
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerInteractor _customerInteractor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="customerInteractor">The customer interactor.</param>
        public CustomerController(ILogger<CustomerController> logger, ICustomerInteractor customerInteractor) : base(logger)
        {
            _logger = logger;
            _customerInteractor = customerInteractor;
        }

        #region Public Endpoints
        /// <summary>
        /// Retrieves all customers as a list of models.
        /// </summary>
        /// <returns>A list of CustomerModel objects.</returns>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetCustomers()
        {
            var res = await _customerInteractor.GetCustomers();
            return await CreateApiResponse(res);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetCustomersPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var res = await _customerInteractor.GetCustomersPaged(page, pageSize, search);
            return await CreateApiResponse(res);
        }

        [HttpGet]
        [Route("suggest")]
        public async Task<IActionResult> SuggestCustomers([FromQuery] string q = "")
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return await CreateApiResponse(new List<MintCart.Api.Customer.Business.Model.CustomerSuggestionModel>());
            var res = await _customerInteractor.SuggestCustomers(q.Trim());
            return await CreateApiResponse(res);
        }

        /// <summary>
        /// Creates a new customer or updates an existing one based on the provided data.
        /// </summary>
        /// <param name="customer">The customer data to create or update.</param>
        /// <returns>The created or updated CustomerModel.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateOrUpdateCustomer([FromBody] CustomerModel customer)
        {
            var res = await _customerInteractor.UpsertCustomer(customer);
            return await CreateApiResponse(res);
        }

        /// <summary>
        /// Merges multiple customer profiles into the primary customer (first ID in the list).
        /// Updates the primary customer's details with the provided information.
        /// </summary>
        /// <param name="mergeRequest">The merge request containing customer IDs and new customer details.</param>
        /// <returns>The updated primary CustomerModel.</returns>
        [HttpPost]
        [Route("merge")]
        public async Task<IActionResult> MergeCustomers([FromBody] MergeCustomerModel mergeRequest)
        {
            var res = await _customerInteractor.MergeCustomers(mergeRequest);
            return await CreateApiResponse(res);
        }
        #endregion
    }
}

