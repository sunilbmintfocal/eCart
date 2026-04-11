namespace MintCart.Api.Core.Exceptions
{
    public class MintCartAuthenticationException : Exception
    {

        public int HttpStatusCode;
        public string AuthenticationExceptionMessage = default!;

        public MintCartAuthenticationException(int httpStatusCode, string authenticationExceptionMessage)
        {
            HttpStatusCode = httpStatusCode;
            AuthenticationExceptionMessage = authenticationExceptionMessage;
        }
    }
}
