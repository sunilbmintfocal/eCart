using MintCart.Common;
using MintCart.Api.Core.ActionResult;
using MintCart.Api.Core.Exceptions;
using MintCart.Api.Core.Response;
using MintCart.Validator;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace MintCart.Api.Core.Filters
{
    public class MintCartExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<MintCartExceptionFilter> _logger;

        private string exceptionMessage = default!;
        private string innerExceptionMessage = default!;
        private string stackTrace = default!;
        private string responseMessage = "";
        private int httpStatusCode = default!;

        public MintCartExceptionFilter(ILogger<MintCartExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if ((context.Exception as MintCartValidationException) != null)
                HandleBussinessValidationException(context);
            else if ((context.Exception as ValidationException) != null)
                HandleValidationException(context);
            else if (context.Exception as MintCartAuthenticationException != null || context.Exception as UnauthorizedAccessException != null)
                HandleAuthenticationExeption(context);
            else
                HandleGeneralException(context);
        }

        /// <summary>
        /// Handle bussiness validation excpetion and construct validation errors and send response
        /// </summary>
        /// <param name="context"></param>
        private void HandleBussinessValidationException(ExceptionContext context)
        {
            _logger.LogError($"Bussiness validation failed. Constructing bad request response with validation error details");
            _logger.LogError($"{context.Exception.Message}");

            List<ApiErrors> errors = new();

            if (context.Exception is MintCartValidationException exception_)
            {
                //Add errors to the model state to handle them automatically in the pipeline
                foreach (var validationsfailures in exception_.Errors)
                {
                    errors.Add(new ApiErrors()
                    {
                        ValidationKey = validationsfailures.ValidationKey,
                        ValidationErrorMessage = validationsfailures.ValidationErrorMessage
                    });
                }
            }

            responseMessage = ResponseMessage.ValidationError;
            context.Result = new MintCartActionResult<dynamic>(responseMessage, errors);
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handle validation excpetion and construct validation errors and send response
        /// </summary>
        /// <param name="context"></param>
        private void HandleValidationException(ExceptionContext context)
        {
            _logger.LogError($"Input validation failed. Constructing bad request response with validation error details");
            _logger.LogError($"{context.Exception.Message}");

            List<ApiErrors> errors = new();

            if (context.Exception is ValidationException exception_)
            {
                //Add errors to the model state to handle them automatically in the pipeline
                //foreach (var validationsfailures in exception_.Errors)
                //{
                //    errors.Add(new ApiErrors()
                //    {
                //        ValidationKey = validationsfailures.PropertyName,
                //        ValidationErrorMessage = validationsfailures.ErrorMessage
                //    });
                //}
            }

            responseMessage = ResponseMessage.ValidationError;
            context.Result = new MintCartActionResult<dynamic>(responseMessage, errors);
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handle authentication excpetion and return unauthorized or forbidden error
        /// </summary>
        /// <param name="context"></param>
        private void HandleAuthenticationExeption(ExceptionContext context)
        {
            if (context.Exception is MintCartAuthenticationException exception_)
            {
                exceptionMessage = $"{exceptionMessage} {exception_.AuthenticationExceptionMessage}";
                responseMessage = exception_.AuthenticationExceptionMessage ?? ResponseMessage.UnAuthorized;
                httpStatusCode = exception_.HttpStatusCode;
            }
            else
            {
                exceptionMessage = $"{exceptionMessage} {context.Exception.Message}";
                responseMessage = context.Exception.Message ?? ResponseMessage.UnAuthorized;
                httpStatusCode = StatusCodes.Status401Unauthorized;
            }
            ApiException exception = new()
            {
                Message = exceptionMessage,
                Code = httpStatusCode
            };

            context.Result = new MintCartAuthorizationResult<dynamic>(exception, responseMessage);
        }

        /// <summary>
        /// Handle general execption and contruction exception details object and send response
        /// </summary>
        /// <param name="context"></param>
        private void HandleGeneralException(ExceptionContext context)
        {
            _logger.LogError($"***********Exception has been logged*************");
            _logger.LogError(context.Exception, context.Exception.Message);

            exceptionMessage = $"{exceptionMessage} {context.Exception.GetBaseException().Message}";
            innerExceptionMessage = $"{context.Exception.GetBaseException().InnerException?.Message}";
            stackTrace = context?.Exception?.StackTrace ?? default!;
            responseMessage = ResponseMessage.Exception;
            httpStatusCode = StatusCodes.Status500InternalServerError;

            ApiException exception = new()
            {
                Message = exceptionMessage,
                StackTrace = stackTrace,
                InnerExcpetion = innerExceptionMessage,
                Code = httpStatusCode
            };

            if (context != null)
                context.Result = new MintCartExceptionResult<dynamic>(exception, responseMessage);
        }

    }
}
