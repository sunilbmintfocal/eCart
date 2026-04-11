using MintCart.Api.Core.ActionResult;
using MintCart.Api.Core.Attribute;
using MintCart.Api.Core.CORS;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MintCart.Api.Core.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	[EnableCors(MintCartApiCorsOptions.CorsOriginPolicy)]
	[Authorize]
	[ModelValidator]
	public class BaseController<T> : ControllerBase
	{
		private readonly ILogger<T> _logger;

		#region constructor
		public BaseController(ILogger<T> logger)
		{
			_logger = logger;
		}
		#endregion

		/// <summary>
		/// Creates a result of an action method
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <param name="response"></param>
		/// <returns>Object of MintCartActionResult</returns>
		public async Task<IActionResult> CreateApiResponse<TModel>(TModel response)
		{
			return await Task.FromResult(new MintCartActionResult<TModel>(response));
		}
	}
}
