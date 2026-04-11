using System.Text.Json.Serialization;

namespace MintCart.Api.Core.Response
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the ApiResponse class
        /// </summary>
        public ApiResponse()
        {
        }
        /// <summary>
        /// Initializes a new instance of the ApiResponse class with Response Data
        /// </summary>
        public ApiResponse(T responseData)
        {
            Data = responseData;
        }
        /// <summary>
        /// Initializes a new instance of the ApiResponse class for successful API response
        /// </summary>
        public ApiResponse(string responseMessage,
            T responseData, int httpStatusCode = 200)
        {
            StatusCode = httpStatusCode;
            Message = responseMessage;
            Data = responseData;
            IsSuccessful = httpStatusCode == 200;
        }
        /// <summary>
        /// Initializes a new instance of the ApiResponse class for API response with Exception
        /// </summary>
        public ApiResponse(string responseMessage, int httpStatusCode, List<ApiErrors> errors)
        {
            StatusCode = httpStatusCode;
            IsSuccessful = false;
            Message = responseMessage;
            Errors = errors;
        }
        /// <summary>
        /// Initializes a new instance of the ApiResponse class for API response with Exception
        /// </summary>
        public ApiResponse(string responseMessage, int httpStatusCode, ApiException responseException)
        {
            StatusCode = httpStatusCode;
            Exception = responseException;
            IsSuccessful = false;
            Message = responseMessage;
        }
        /// <summary>
        /// Gets or sets the status code of Http response
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the response message explaining the staus code
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// Gets or sets value that indicates if the response was successful
        /// </summary>
        public bool IsSuccessful { get; set; }
        /// <summary>
        /// Gets or sets the content of API response message
        /// </summary>
        public T Data { get; set; } = default!;
        /// <summary>
        /// Gets or sets a message that describes the current exception
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApiException Exception { get; set; } = default!;

        /// <summary>
        /// Gets or sets the error details for the API
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<ApiErrors> Errors { get; set; } = default!;
    }
}
