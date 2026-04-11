using MintCart.Logging;
using MintCart.Security;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MintCart.Configuration.AppSettingsHelper
{
    public class GCPSecretReader : BaseReader
    {
        public GCPSecretReader(IConfiguration configuration, EncryptionHelper ss
            , ILogger<BaseReader> logger) :base(configuration, ss, logger)
        {
        }

        /// <summary>
        /// Get app settings by key from cache or 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns>returns app setting value</returns>
        public override ConfigurationResponse<T> Get<T>(string key, T defaultValue)
        {
            _logger.StartMethodLog("GCP Secret Get :" + key);
            try
            {
                SecretManagerServiceClient client = SecretManagerServiceClient.Create();
                _logger.Info("Secret client created");
                SecretVersionName versionName = new SecretVersionName(MintCartConfigConstants.GoogleSecretProjectId
                        , key, MintCartConfigConstants.GoogleSecretVersion);
                _logger.Info("Start accessing secret data");
                AccessSecretVersionResponse response = client.AccessSecretVersion(versionName);
                var result = response.Payload.Data.ToStringUtf8();
                _logger.Info("End accessing secret data");
                if (string.IsNullOrEmpty(result))
                {
                    _logger.EndMethodLog("GCP Secret Get Default:" + key);
                    return new ConfigurationResponse<T>((T)Convert.ChangeType(defaultValue, typeof(T)), false);
                }
                var value = (T)Convert.ChangeType(result, typeof(T));
                _logger.EndMethodLog("GCP Secret Get :" + key);
                return new ConfigurationResponse<T>(value, true);
            }
            catch (Exception ex)
            {
                _logger.LogError("GCP Secret Error", ex);
                return new ConfigurationResponse<T>((T)Convert.ChangeType(defaultValue, typeof(T)), false);
            }
        }
    }
}
