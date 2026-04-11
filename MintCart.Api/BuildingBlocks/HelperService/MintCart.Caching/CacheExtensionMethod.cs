using Microsoft.Extensions.DependencyInjection;
namespace MintCart.Caching
{
    public static class CacheExtensionMethod
    {
        public static void AddInfraServices(this IServiceCollection services)
        {
            services.AddScoped<ICacheManager, RedisCacheManager>();
        }

    }
}
