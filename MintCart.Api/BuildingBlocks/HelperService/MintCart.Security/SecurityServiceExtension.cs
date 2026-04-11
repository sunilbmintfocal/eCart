using Microsoft.Extensions.DependencyInjection;

namespace MintCart.Security
{
	public static class SecurityServiceExtension
    {
        public static void AddSecurity(this IServiceCollection services)
        {
            services.AddSingleton<EncryptionHelper>();
        }
    }
}
