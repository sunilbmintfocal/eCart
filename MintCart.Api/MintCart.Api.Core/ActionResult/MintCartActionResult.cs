using MintCart.Api.Core.Response;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;
using MintCart.Common;

namespace MintCart.Api.Core.ActionResult
{
    /// <summary>
    /// Represents a base custom action result for the MintCart API.
    /// </summary>
    public class MintCartActionResult : IActionResult
    {
        /// <summary>
        /// Executes the action result asynchronously.
        /// </summary>
        /// <param name="context">The action context.</param>
        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Represents a generic custom action result for the MintCart API, encapsulating a specific type of data.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned.</typeparam>
    public class MintCartActionResult<T> : MintCartActionResult
    {
        /// <summary>
        /// The API response data.
        /// </summary>
        public ApiResponse<T> Data = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="MintCartActionResult{T}"/> class with success data.
        /// </summary>
        /// <param name="data">The data to return.</param>
        public MintCartActionResult(T data)
        {
            CreateMintCartApiResponse(data, Status200OK, ResponseMessage.Success);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MintCartActionResult{T}"/> class with errors.
        /// </summary>
        /// <param name="responseMessage">The error message.</param>
        /// <param name="errors">The list of API errors.</param>
        public MintCartActionResult(string responseMessage, List<ApiErrors> errors)
        {
            Data = new ApiResponse<T>(responseMessage, Status400BadRequest, errors);
        }

        /// <summary>
        /// Executes the action result asynchronously by writing the response to the output.
        /// </summary>
        /// <param name="context">The action context.</param>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(Data)
            {
                StatusCode = Data.StatusCode
            };
            await objectResult.ExecuteResultAsync(context);
        }

        /// <summary>
        /// Helper method to create the internal API response object.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="responseMessage">The response message.</param>
        private void CreateMintCartApiResponse(T data, int httpStatusCode, string responseMessage)
        {
            Data = new ApiResponse<T>(responseMessage, data, httpStatusCode);
        }
    }
}
