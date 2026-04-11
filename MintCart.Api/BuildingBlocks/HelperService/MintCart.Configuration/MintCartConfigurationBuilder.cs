using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace MintCart.Configuration
{
	public static class MintCartConfigurationBuilder
    {
        public static void BuildConfiguration(IConfigurationBuilder configurationBuilder, string environment)
        {
            string _coreAppSettingJson = "coreappsettings.json";
            string _appSettingJson = "appsettings.json";

            string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? string.Empty) ?? string.Empty;
            Console.WriteLine(basePath);

            //To add migrations from package manager console - for development
            if (environment == "Development")
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                if (!File.Exists(Path.Combine(basePath, _coreAppSettingJson)))
                    _coreAppSettingJson = Path.Combine(currentDirectory.Replace("MintCart.API", "MintCart.Core.API"), _coreAppSettingJson);
                if (!File.Exists(Path.Combine(basePath, _appSettingJson)))
                    _appSettingJson = Path.Combine(currentDirectory, _appSettingJson);
            }

            configurationBuilder
                .SetBasePath(basePath)
                .AddJsonFile(_coreAppSettingJson, optional: false, reloadOnChange: true)
                .AddJsonFile($"coreappsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddJsonFile(_appSettingJson, optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            configurationBuilder.AddEnvironmentVariables();
        }
    }
}
