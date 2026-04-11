namespace MintCart.Api.Core.CORS
{
    public class MintCartApiCorsOptions
    {
        public const string CorsOriginPolicy = "MintCartCorsOriginPolicy";
        public static readonly string[] AllowedMethods = { "PUT", "DELETE", "GET", "POST" };
    }
}
