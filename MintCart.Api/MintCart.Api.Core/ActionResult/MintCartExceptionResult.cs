using MintCart.Api.Core.Response;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MintCart.Api.Core.ActionResult
{
    public class MintCartExceptionResult<T> : MintCartActionResult<T>
    {
        public MintCartExceptionResult(ApiException exception, string responseMessage) : base(default(T) ?? default!)
        {
            CreateMintCartApiExceptionResponse(exception, Status500InternalServerError, responseMessage);
        }
        private void CreateMintCartApiExceptionResponse(ApiException exception, int httpStatusCode, string responseMessage)
        {
            Data = new ApiResponse<T>(responseMessage, httpStatusCode, exception);
        }
    }
}

