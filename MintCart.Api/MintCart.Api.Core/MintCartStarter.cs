using MintCart.Configuration;

namespace MintCart.Api.Core
{
	public class MintCartStarter
	{
		public static void Main(string[] args)
		{
			CreateABBuilder(args).Build().Run();
		}

		public static WebApplicationBuilder CreateABBuilder(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
			{
				MintCartConfigurationBuilder.BuildConfiguration(config, builder.Environment.EnvironmentName);
			});
			return builder;
		}
	}
}
