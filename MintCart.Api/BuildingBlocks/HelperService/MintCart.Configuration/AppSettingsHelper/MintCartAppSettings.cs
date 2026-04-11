
using MintCart.Common;
using MintCart.Common.Constants;
using MintCart.Security;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MintCart.Configuration.AppSettingsHelper
{
    public class MintCartAppSettings : IMintCartAppSettings
    {
        private readonly IConfiguration _configuration;
        readonly EncryptionHelper ss;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<BaseReader> _logger;

        public MintCartAppSettings(IConfiguration configuration, EncryptionHelper ss
            , IHostingEnvironment env, ILogger<BaseReader> logger)
        {
            _configuration = configuration;
            this.ss = ss;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Get a configuration from Appsetting or gcp secret
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isSecret"></param>
        /// <returns></returns>
        public ConfigurationResponse<T> Get<T>(string key, T defaultValue)
        {
            bool isSecret = false;
            var secretValuesStr = _configuration.GetValue<string>(AppSettingConstants.SecretKeys);
            if (!string.IsNullOrEmpty(secretValuesStr))
            {
                var secretValues = secretValuesStr.Split(",").ToList();
                if (secretValues != null && secretValues.Contains(key))
                {
                    isSecret = true;
                }
            }
            var appSettingsReader = GetAppSettingsReader(isSecret);
            var value = appSettingsReader.Get(key, defaultValue);
            return value;
        }

        #region Private Methods

        private BaseReader GetAppSettingsReader(bool isSecret)
        {
            var envName = _env.EnvironmentName;
            if (envName != MintCartConfigConstants.LocalEnvironmentName && isSecret)
            {
                return new GCPSecretReader(_configuration, ss, _logger);
            }
            return new AppSettingsReader(_configuration, ss, _logger);
        }

        #endregion
    }
}
