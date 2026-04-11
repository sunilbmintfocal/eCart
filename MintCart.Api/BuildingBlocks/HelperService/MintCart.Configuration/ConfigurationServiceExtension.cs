using MintCart.Configuration.AppSettingsHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Configuration
{
    public static class ConfigurationServiceExtension
    {
        public static void AddConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IMintCartAppSettings, MintCartAppSettings>();
        }
    }
}
