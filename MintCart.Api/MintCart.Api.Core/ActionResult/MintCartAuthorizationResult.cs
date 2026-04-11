using MintCart.Api.Core.Response;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MintCart.Api.Core.ActionResult
{
    public class MintCartAuthorizationResult<T> : MintCartActionResult<T>
    {
        public MintCartAuthorizationResult(ApiException apiException, string responseMessage) : base(default(T) ?? default!)
        {
            CreateApiAthorizationResponse(apiException, Status401Unauthorized, responseMessage);
        }

        private void CreateApiAthorizationResponse(ApiException apiException, int httpStatusCode, string responseMessage)
        {
            Data = new ApiResponse<T>(responseMessage, httpStatusCode, apiException);
        }
    }
}
