using MintCart.Security;
using Google.Apis.Logging;
using Microsoft.Extensions.Configuration;
using MintCart.Logging;
using Microsoft.Extensions.Logging;

namespace MintCart.Configuration.AppSettingsHelper
{
    public abstract class BaseReader
    {
        protected readonly IConfiguration _configuration;
        protected readonly EncryptionHelper ss;
        protected readonly ILogger<BaseReader> _logger;

        protected BaseReader(IConfiguration configuration, EncryptionHelper ss
            , ILogger<BaseReader> logger)
        {
            _configuration = configuration;
            this.ss = ss;
            _logger = logger;
        }

        /// <summary>
        /// Get app settings by key from cache or 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Configuration Key representing the value</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>returns app setting value</returns>
        public abstract ConfigurationResponse<T> Get<T>(string key, T defaultValue);
    }
}
