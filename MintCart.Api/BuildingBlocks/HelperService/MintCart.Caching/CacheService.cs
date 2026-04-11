using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MintCart.Caching.Redis;

namespace MintCart.Caching
{
    public static class CacheService
    {
        public static void ConfigureCacheService(this IServiceCollection services, string redisConnectionString)
        {
            services.AddSingleton<ICacheManager,RedisCacheManager>(connection =>
             {
                 var logger = connection.GetService<ILogger<RedisCacheManager>>();
                 logger.LogDebug("Redis Connectionstring : {redisConnectionString}", redisConnectionString);
                 var redisConfig = new RedisConfig()
                 {
                     ConnectionString = redisConnectionString
				 };
                 var redisConnectionWrapper = new RedisConnectionWrapper(redisConfig);
                 return new RedisCacheManager(redisConnectionWrapper,logger, redisConfig);
             });

        }
    }
}
