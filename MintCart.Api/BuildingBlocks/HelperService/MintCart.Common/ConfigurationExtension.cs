using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Common
{
    public static class ConfigurationExtension
    {
        public static T GetAppSetting<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetValue<T>(key);
        }
        public static T GetAppSetting<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            return configuration.GetValue<T>(key, defaultValue);
        }
        public static List<T> GetAppSection<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T[]>().ToList();
        }
        public static bool IsKeyExists(this IConfiguration configuration, string keyName)
        {
            var keyNames = configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            return keyNames.ContainsKey(keyName) ? true : false;
        }
    }
}
