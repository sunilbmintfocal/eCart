namespace MintCart.Configuration.AppSettingsHelper
{
    public interface IMintCartAppSettings
    {
        /// <summary>
        /// Get app settings by key from cache or 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns>returns app setting value</returns>
        ConfigurationResponse<T> Get<T>(string key, T defaultValue);
    }
}
