using MintCart.Common;
using MintCart.Configuration;

namespace MintCart.Api.Core.CORS
{
    public static class MintCartApiCorsExtension
    {
        public static void AddMintCartCorsPolicy(this IServiceCollection services, IConfiguration configuration, Serilog.ILogger logger)
        {
            var allowdedHost = configuration.GetAppSection<string>(MintCartConfigConstants.AllowedHosts);

            logger.Debug($"Adding Cors Policy. Allowded Hosts : {CommonHelper.ListToString(allowdedHost)}");


            services.AddCors(options =>
            {
                options.AddPolicy(name: MintCartApiCorsOptions.CorsOriginPolicy,
                                  policy =>
                                  {
                                      policy.SetIsOriginAllowed(origin =>
                                      {
                                          if (allowdedHost.Count() > 0)
                                          {
                                              if (allowdedHost.Contains("*"))
                                              {
                                                  return true;
                                              }
                                              else if (allowdedHost.Contains(origin))
                                              {
                                                  return true;
                                              }
                                              else
                                              {
                                                  return false;
                                              }
                                          }
                                          else
                                          {
                                              return false;
                                          }
                                      }) //SignalR connections will not accept '*'
                                      .WithMethods(MintCartApiCorsOptions.AllowedMethods)
                                      .AllowAnyHeader()
                                      .AllowCredentials(); // Required for SignalR
                                  });
            });

            logger.Information($"Added AddPolicy ,with allowded Host {CommonHelper.ListToString(allowdedHost)}");
        }
    }
}
