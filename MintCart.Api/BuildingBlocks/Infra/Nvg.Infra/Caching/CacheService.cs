using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nvg.Infra.Redis;

namespace Nvg.Infra.Caching
{
    public static class CacheService
    {
        public static void ConfigureCacheService(this IServiceCollection services)
        {
            services.AddSingleton<ICacheManager, RedisCacheManager>(connection =>
            {
                var logger = connection.GetService<ILogger<RedisCacheManager>>();
                var configuration = connection.GetService<IConfiguration>();
                string connectionString = configuration.GetValue<string>("RedisConnectionString");
                logger.LogDebug("Redis Connectionstring : {connectionString}", connectionString);
                var redisConfig = new RedisConfig()
                {
                    ConnectionString = connectionString
                };
                var redisConnectionWrapper = new RedisConnectionWrapper(redisConfig);
                return new RedisCacheManager(redisConnectionWrapper, logger, redisConfig);
            });

        }
    }
}
