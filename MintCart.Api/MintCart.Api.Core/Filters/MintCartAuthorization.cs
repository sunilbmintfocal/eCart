using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using MintCart.Common;
using MintCart.Api.Core.Response;
using MintCart.Api.Core.ActionResult;

namespace MintCart.Api.Core.Filters
{
    public class MintCartAuthorization : AuthorizeAttribute, IAuthorizationFilter
    {
        private string responseMessage = "";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (IsValidToken())
                return;
            else
            {
                ApiException apiException = new()
                {
                    Message = ResponseMessage.UnAuthorized,
                    Code = StatusCodes.Status401Unauthorized
                };
                responseMessage = $"{ResponseMessage.UnAuthorized}";

                context.Result = new MintCartAuthorizationResult<dynamic>(apiException, responseMessage);
            }
        }

        private bool IsValidToken()
        {
            /**
             * Authorization is handled by Jwt Bearer Default Authentication Scheme defined in LMWindABServiceConfiguration.
             * Hence by passing the authentication and returning true
             * */
            return true;
        }
    }
}
