using Microsoft.Extensions.DependencyInjection;
using Nvg.Infra.Caching;
using Nvg.Infra.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nvg.Infra.Extensions
{
    public static class CustomExtensionMethod
    {
        public static void AddInfraServices(this IServiceCollection services, string redisConString)
        {
            services.AddScoped<ICacheManager, RedisCacheManager>();
            services.AddScoped<IRedisConnectionWrapper, RedisConnectionWrapper>();
            services.Configure<RedisConfig>(rc => rc.ConnectionString = redisConString);
        }
    }
}
