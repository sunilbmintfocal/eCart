using Microsoft.Extensions.DependencyInjection;
namespace MintCart.Caching.Redis
{
    public static class RedisExtensionMethod
    {
        public static void AddInfraServices(this IServiceCollection services,string redisConString)
        {
            services.AddScoped<IRedisConnectionWrapper, RedisConnectionWrapper>();
            services.Configure<RedisConfig>(rc => rc.ConnectionString = redisConString);
        }

    }
}
