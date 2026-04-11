using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;

namespace MintCart.Logging
{
	public static class ServiceCollectionExtension
    {
        public static ILogger CreateMintCartLogger(this IServiceCollection services, IConfiguration configuration)
        {
            var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console(new RenderedCompactJsonFormatter())
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(configuration).Enrich.With<MintCartLogEnricher>()
            .CreateLogger();

            Log.Logger = serilogLogger;

            return serilogLogger;
        }
    }
}
