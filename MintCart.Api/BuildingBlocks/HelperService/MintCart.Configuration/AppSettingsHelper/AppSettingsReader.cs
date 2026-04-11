using MintCart.Common;
using MintCart.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using MintCart.Logging;

namespace MintCart.Configuration.AppSettingsHelper
{
    public class AppSettingsReader : BaseReader
    {
        public AppSettingsReader(IConfiguration configuration, EncryptionHelper ss
            , ILogger<BaseReader> logger) : base(configuration, ss, logger)
        {
        }

        /// <summary>
        /// Get app settings by key from cache or 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns>returns app setting value</returns>
        public override ConfigurationResponse<T> Get<T>( string key, T defaultValue)
        {
            _logger.StartMethodLog("AppSettings Get :" + key);
            if (_configuration.IsKeyExists(key))
            {
                var value = _configuration.GetAppSetting<T>(key) ?? default;
                if (_configuration.IsKeyExists(key + "IsEncrypted") && typeof(T).Name.ToLower() == "string")
                {
                    if (_configuration.GetAppSetting<bool>(key + "IsEncrypted"))
                        value = (T)Convert.ChangeType(ss.DecryptString_Aes(value.ToString()), typeof(T));
                }
                _logger.EndMethodLog("AppSettings Get :" + key);
                return new ConfigurationResponse<T>(value, true);
            }
            _logger.EndMethodLog("AppSettings Get Default:" + key);
            return new ConfigurationResponse<T>(defaultValue, false);
        }
    }
}
