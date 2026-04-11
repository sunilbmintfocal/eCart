using MintCart.Common;
using MintCart.Api.Core.Exceptions;
using MintCart.Api.Core.Response;
using System.Text;
using System.Text.Json;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MintCart.Api.Core.Middleware
{
    public class RequestResponseHelperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseHelperMiddleware> _logger;
        private string swaggerPath { get; set; } = "/swagger";
        private string grpcPath { get; set; } = "/GRPC.";

        #region Constructor
        public RequestResponseHelperMiddleware(RequestDelegate next, ILogger<RequestResponseHelperMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        #endregion
        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context, swaggerPath) || !IsApi(context) || IsGRPC(context, grpcPath))
                await _next(context);
            else
            {
                LogRequestStarted();

                using var memoryStream = new MemoryStream();

                var originalResponseBodyStream = context.Response.Body;
                try
                {
                    _logger.LogInformation($"Requested URL :{context.Request.Path}, Method :{context.Request.Method}");

                    if (context.Request.Method.ToUpper() != "GET")
                    {
                        string requestData = await GetRequestBodyAsync(context.Request);

                        _logger.LogDebug($"Request Data - {requestData}");

                        if (!string.IsNullOrWhiteSpace(requestData) &&
                            !requestData.ToString().IsNullOrEmptyJson() && !requestData.ToString().IsValidJson())
                        {
                            //Throw the bad request for invalid Json in request data
                            await HandleUnsuccessfulRequestAsync(context, "Invalid Json in Request Body", Status400BadRequest);

                            await RevertResponseBodyStreamAsync(memoryStream, originalResponseBodyStream);

                            return;
                        }
                    }
                    await _next.Invoke(context);

                    if (context.Response.HasStarted)
                    {
                        return;
                    }

                    /**
                   * Response for Http Status Code 200,400,401,500 will be handled by Action Filters
                   * Other status code except 204 and 304 
                   * and not in the range of Status code >=200 && Status code <400
                   * is considered as unsuccessful request which will be handled by  Middleware
                   */

                    bool isRequestOk = IsRequestSuccessful(context.Response.StatusCode);
                    if (context.Response.StatusCode != Status304NotModified &&
                        context.Response.StatusCode != Status204NoContent &&
                        context.Response.StatusCode != Status400BadRequest &&
                        context.Response.StatusCode != Status401Unauthorized &&
                        context.Response.StatusCode != Status500InternalServerError && !isRequestOk)
                    {
                        /**
                         * Read the current stream and get all the characters as parsed text 
                         */
                        context.Response.Body = originalResponseBodyStream;
                        var bodyAsText = await ReadResponseBodyStreamAsync(memoryStream);
                        await HandleUnsuccessfulRequestAsync(context, bodyAsText, context.Response.StatusCode);
                    }

                }
                catch (Exception ex)
                {
                    if (context.Response.HasStarted)
                    {
                        return;
                    }

                    await HandleExceptionAsync(context, ex);
                    /**
                     * Restore the original response stream to current Http Context 
                     */
                    await RevertResponseBodyStreamAsync(memoryStream, originalResponseBodyStream);
                }
                finally
                {
                    LogRequestEnd();
                }
            }
        }
        #region Public Methods

        #endregion

        #region Private Methods
        /// <summary>
        /// This method will processes the http request with exception asynchronously
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="requestTimeStamp"></param>
        /// <returns>
        /// Returns a task to write the formatted Response to HttpContext asynchronously
        /// </returns>
        public async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            ApiException apiException = new();

            string exceptionMessage = default!;
            string innerExceptionMessage = default!;
            int httpStatusCode;
            bool showExceptionDetails = true;
            string responseMessage;

            if (exception is UnauthorizedAccessException)
            {
                /**
                 * Setting API Exception for Unauthorized Access
                 */
                apiException.Message = ResponseMessage.UnAuthorized;
                httpStatusCode = Status401Unauthorized;
                responseMessage = $"{context.Request.Method} {ResponseMessage.UnAuthorized}";
            }
            else if (exception is MintCartAuthenticationException)
            {
                MintCartAuthenticationException exception_ = (MintCartAuthenticationException)exception;
                apiException.Message = exception_.AuthenticationExceptionMessage;
                httpStatusCode = exception_.HttpStatusCode;
                responseMessage = $"{context.Request.Method} {exception_.AuthenticationExceptionMessage}";
            }
            else
            {
                string stackTrace = default!;

                /**
                 * Constructing API Exception
                 * Stack Trace will be displayed if ShowExceptionDetails is true
                 * Default is set to true for ShowExceptionDetails in RestWrapperOptions class
                 * Http Status will be set to Internal Server Error
                 */
                if (showExceptionDetails)
                {
                    exceptionMessage = $"{exceptionMessage} {exception.GetBaseException().Message}";
                    innerExceptionMessage = $"{exception.GetBaseException().InnerException?.Message}";
                    stackTrace = exception.StackTrace ?? "";
                    responseMessage = $"{context.Request.Method} {ResponseMessage.Exception}";
                }
                else
                {
                    exceptionMessage = ResponseMessage.Unhandled;
                    responseMessage = $"{context.Request.Method} {ResponseMessage.Unhandled}";
                }

                apiException.Message = exceptionMessage;
                apiException.StackTrace = stackTrace;
                apiException.InnerExcpetion = innerExceptionMessage;
                httpStatusCode = Status500InternalServerError;
            }

            var errorMessage = ResponseMessage.Exception;
            _logger.LogError($"***********Exception has been logged*************");
            _logger.LogError(exception, $"[{httpStatusCode}]: {errorMessage}");

            /**
            * Serialize API response with Exception and Transaction details
            * Call Task to write the API exception to current context's response body
            */
            ApiResponse<dynamic> apiResponse = new()
            {
                StatusCode = httpStatusCode,
                Message = responseMessage,
                Exception = apiException,
            };

            var jsonString = ConvertToJSONString(apiResponse);
            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        /// <summary>
        /// This method will processes the unsuccessful http Request asynchronously
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        /// <param name="httpStatusCode"></param>
        /// <returns>
        /// Returns a task to write the formatted Response to HttpContext asynchronously
        /// </returns>
        public async Task HandleUnsuccessfulRequestAsync(HttpContext context, object body, int httpStatusCode)
        {
            /**
             * Parse the content of response body to JSON string
             */
            var (IsEncoded, ParsedText) = body.ToString().VerifyBodyContent();

            /**
            * Deserialize the parsed content of response body and 
            * Construct  API Exception Message from http status code for a unsuccessful request
            */
            var bodyText = IsEncoded ? JsonSerializer.Deserialize<dynamic>(ParsedText) : body.ToString();
            ApiException apiException = new ApiException()
            {
                Message = !string.IsNullOrEmpty(body?.ToString()) ? bodyText ?? "" : WrapUnsuccessfulError(httpStatusCode)
            };
            var responseMessage = $"{context.Request.Method} {apiException.Message}";

            var errorMessage = ResponseMessage.Exception;
            _logger.LogError(apiException.Message, $"[{httpStatusCode}]: {errorMessage}");

            /**
             * Serialize API response with Exception and Transaction details
             * Call Task to write the API exception to current context's response body
             */
            var ApiResponse = new ApiResponse<dynamic>()
            {
                StatusCode = httpStatusCode,
                Message = responseMessage,
                Exception = apiException,
            };

            var jsonString = ConvertToJSONString(ApiResponse);
            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }
        /// <summary>
        /// This method will check if the Http response was successful
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns>
        /// Returns true if the status code is in the range  of HTTP success code list; Otherwise false
        /// </returns>
        public bool IsRequestSuccessful(int statusCode)
        {
            return (statusCode >= 200 && statusCode < 400);
        }
        /// <summary>
        /// This method will check if the request is from API project only
        /// </summary>
        /// <param name="context"></param>
        /// <returns>
        /// Returns true if the request is made from web api; Otherwise false
        /// </returns>
        public bool IsApi(HttpContext context)
        {
            string path = context?.Request?.Path.Value ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(path)
                && !path.Contains(".js")
                && !path.Contains(".css")
                && !path.Contains(".html"))
                return true;
            else
                return false;
        }
        /// <summary>
        /// This method will check if the request is from Swagger URL
        /// </summary>
        /// <param name="context"></param>
        /// <param name="swaggerPath"></param>
        /// <returns>
        /// Returns true if the request is made from swagger endpoint; Otherwise false
        /// </returns>
        public bool IsSwagger(HttpContext context, string swaggerPath)
        {
            return context.Request.Path.StartsWithSegments(new PathString(swaggerPath));
        }
        public bool IsGRPC(HttpContext context, string grpcPath)
        {
            return context.Request.Path.Value.StartsWith(new PathString(grpcPath));
        }
        private void LogRequestStarted()
        {
            _logger.LogInformation($"***************API-Request-START***************");
        }
        private void LogRequestEnd()
        {
            _logger.LogInformation($"***************API-Request-END***************{Environment.NewLine}");
        }
        /// <summary>
        /// This method will serialize the APIResponse to JSON string 
        /// Custom JSON Serialize Settings will be used
        /// </summary>
        /// <param name="ApiResponse"></param>
        /// <returns></returns>
        private string ConvertToJSONString(ApiResponse<dynamic> ApiResponse)
        {
            return RestHelper.ConvertToJSONString(ApiResponse);
        }
        /// <summary>
        /// This method will return the response message for unsuccessful HTTP Response
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private object WrapUnsuccessfulError(int statusCode) =>
            statusCode switch
            {
                Status204NoContent => ResponseMessage.NotContent,
                Status400BadRequest => ResponseMessage.BadRequest,
                Status401Unauthorized => ResponseMessage.UnAuthorized,
                Status404NotFound => ResponseMessage.NotFound,
                Status405MethodNotAllowed => ResponseMessage.MethodNotAllowed,
                Status415UnsupportedMediaType => ResponseMessage.MediaTypeNotSupported,
                _ => ResponseMessage.Unknown
            };
        /// <summary>
        /// This method will writes the JSON string to the response body
        /// UTF-8 encoding will be used
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="context"></param>
        /// <param name="jsonString"></param>
        private async Task WriteFormattedResponseToHttpContextAsync(HttpContext context, int httpStatusCode, string jsonString)
        {
            context.Response.StatusCode = httpStatusCode;
            context.Response.ContentType = TypeIdentifier.JSONHttpContentMediaType;
            context.Response.ContentLength = jsonString != null ? Encoding.UTF8.GetByteCount(jsonString) : 0;
            await context.Response.WriteAsync(jsonString ?? "");
        }
        /// <summary>
        /// This method will  Read characters from the current stream asynchronously and writes them to another
        /// </summary>
        /// <param name="bodyStream"></param>
        /// <param name="orginalBodyStream"></param>
        /// <returns></returns>
        public async Task RevertResponseBodyStreamAsync(Stream bodyStream, Stream orginalBodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            await bodyStream.CopyToAsync(orginalBodyStream);
        }
        /// <summary>
        /// This method will  Read characters from the current stream asynchronously
        /// </summary>
        /// <param name="bodyStream"></param>
        /// <returns>
        /// Returns the current stream as string or parsed string 
        /// </returns>
        public async Task<string> ReadResponseBodyStreamAsync(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);

            var (IsEncoded, ParsedText) = responseBody.VerifyBodyContent();

            return IsEncoded ? ParsedText : responseBody;
        }
        /// <summary>
        /// This method will  Read characters from the current request body
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Returns the current request body as string
        /// </returns>
        public async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            using var memoryStream = new MemoryStream();
            await request.Body.CopyToAsync(memoryStream);
            string? requestBody = Encoding.UTF8.GetString(memoryStream.ToArray());
            request.Body.Seek(0, SeekOrigin.Begin);

            return requestBody ?? "";
        }
        #endregion
    }
}
