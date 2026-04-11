using MintCart.Api.Core.Response;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;
using MintCart.Common;

namespace MintCart.Api.Core.ActionResult
{
    public class MintCartActionResult : IActionResult
    {
        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            await ExecuteResultAsync(context);
        }
    }

    public class MintCartActionResult<T> : MintCartActionResult
    {
        public ApiResponse<T> Data = default!;

        public MintCartActionResult(T data)
        {
            CreateMintCartApiResponse(data, Status200OK, ResponseMessage.Success);
        }

        public MintCartActionResult(string responseMessage, List<ApiErrors> errors)
        {
            Data = new ApiResponse<T>(responseMessage, Status400BadRequest, errors);
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(Data)
            {
                StatusCode = Data.StatusCode
            };
            await objectResult.ExecuteResultAsync(context);
        }

        private void CreateMintCartApiResponse(T data, int httpStatusCode, string responseMessage)
        {
            Data = new ApiResponse<T>(responseMessage, data, httpStatusCode);
        }
    }
}
