using Microsoft.Extensions.DependencyInjection;

namespace MintCart.Identity
{
	public static class IdentityServiceExtension
    {
        public static void AddIdentityHelper(this IServiceCollection services)
        {
            services.AddScoped<EventUserContext>(); 
            services.AddScoped<IUserContext, UserContext>();
        }
    }
}
