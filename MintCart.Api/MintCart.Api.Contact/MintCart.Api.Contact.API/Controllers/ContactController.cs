using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using MintCart.Api.Contact.Business.Interface;
using MintCart.Api.Contact.Business.Model;
using MintCart.Api.Core.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MintCart.Api.Contact.API.Controllers
{
    [Route("api/contact")]
    public class ContactController : BaseController<ContactController>
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactInteractor _contactInteractor;

        public ContactController(ILogger<ContactController> logger, IContactInteractor contactInteractor) : base(logger)
        {
            _logger = logger;
            _contactInteractor = contactInteractor;
        }

        /// <summary>
        /// Gives all the Contacts as a list of objects
        /// </summary>
        /// <returns>Returns the list of Contacts</returns>
        [HttpGet]
        [Route("contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var res = await _contactInteractor.GetContacts();
            return await CreateApiResponse(res);
        }
    }
}
