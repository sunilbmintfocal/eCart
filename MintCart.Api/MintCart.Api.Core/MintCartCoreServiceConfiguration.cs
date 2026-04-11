using MintCart.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using ILogger = Serilog.ILogger;
using MintCart.Security;
using MintCart.Configuration;
using MintCart.Api.Core.CORS;
using MintCart.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using MintCart.Api.Core.Middleware;
using MintCart.Api.Core.Filters;
// using MintCart.Api.Core.GRPC;
using MintCart.Api.Core.Response;
using MintCart.Identity;
using Microsoft.AspNetCore.Mvc;
using MintCart.Common.Constants;
using MintCart.Configuration.AppSettingsHelper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;

namespace MintCart.Api.Core
{
    public static class MintCartCoreServiceConfiguration
    {
        private static readonly string swaggerBasePath = "mintcart";
        private static readonly string eventStreamUrl = "/mintcart/livestream/hubs/event";
        public static IServiceCollection ConfigureCoreMintCartService(this WebApplicationBuilder webApplicationBuilder)
        {
            var services = webApplicationBuilder.Services;
            var configuration = webApplicationBuilder.Configuration;

            ILogger logger = services.CreateMintCartLogger(configuration);
            webApplicationBuilder.Host.UseSerilog(logger);
            services.AddScoped<MintCartAuthorization>();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(MintCartActionFilter));
                options.Filters.Add(typeof(MintCartExceptionFilter));
            }).AddJsonOptions(op =>
            {
                //op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                op.JsonSerializerOptions.WriteIndented = true;
            });


            AddSwaggerGenConfiguration(services);

            services.AddSecurity();

            services.AddConfiguration();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMintCartCorsPolicy(configuration, logger);

            ConfigureIdentityServer(services);

            services.AddAuthorization();

            services.AddValidationHelper();

            services.AddIdentityHelper();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHealthChecks();

            // services.AddScoped<GrpcCallerService>();

            services.AddControllers().AddApplicationPart(System.Reflection.Assembly.GetEntryAssembly());

            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }
        public static void ConfigureMintCartCore(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseMiddleware<RequestResponseHelperMiddleware>();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = swaggerBasePath + "/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{swaggerBasePath}/swagger/v1/swagger.json", $"APP API - V1");
                c.RoutePrefix = $"{swaggerBasePath}/swagger";
            });



            //HealthCheck Middleware
            app.MapHealthChecks("/api/startup");

            app.MapHealthChecks("/api/ready", new HealthCheckOptions { Predicate = _ => false });

            app.MapHealthChecks("/api/health");

            app.UseCors(MintCartApiCorsOptions.CorsOriginPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
        }
        #region "Private Methods"
        private static void AddSwaggerGenConfiguration(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MintCart", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme,Id = "Bearer"}},
                        new string[] { }
                    }
                });
            });
        }
        private static void ConfigureIdentityServer(IServiceCollection services)
        {

            var builder = services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme);

            builder.AddJwtBearer(options => SetJwtBearerOptions(options, services));

        }
        private static void SetJwtBearerOptions(JwtBearerOptions options, IServiceCollection services)
        {
            options.Authority = services.GetIdentityUrl();
            //options.Audience = services.GetIdentityAudience();
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    //c.NoResult();
                    //c.Response.ContentType = "text/plain";
                    //return c.Response.WriteAsync("Not authorized.: " + c.Exception.ToString());
                    c.Response.OnStarting(async () =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        c.Response.ContentType = "application/json";

                        var ipAddress = c.HttpContext.Connection.RemoteIpAddress;
                        var Path = c.HttpContext.Request.Path.ToString();
                        var Query = c.HttpContext.Request.Query.ToString();
                        var Body = c.HttpContext.Request.Body.ToString();
                        Console.WriteLine("IP: " + ipAddress + " | " + " Path: " + Path + " | Query: " + Query + " | Body: " + Body);

                        string response = JsonConvert.SerializeObject(new ApiResponse<string>("The access token provided has expired.", 401, new List<ApiErrors>() { new ApiErrors { ValidationKey = "Authentication", ValidationErrorMessage = "The access token provided is not valid." } }));
                        if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            c.Response.Headers.Add("Token-Expired", "true");
                            response = JsonConvert.SerializeObject(new ApiResponse<string>("The access token provided has expired.", 401, new List<ApiErrors>() { new ApiErrors { ValidationKey = "Authentication", ValidationErrorMessage = "The access token provided has expired." } }));
                        }
                        await c.Response.WriteAsync(response);
                    });
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    if (context.Request.Query.TryGetValue("access_token", out StringValues accessToken))
                    {
                        var path = context.HttpContext.Request.Path;
                        if (!StringValues.IsNullOrEmpty(accessToken) && (path.StartsWithSegments(eventStreamUrl)))
                        {
                            context.Token = accessToken;
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        }

        private static string GetIdentityUrl(this IServiceCollection services)
        {
            IServiceProvider serviceProviderservicesProvider = services.BuildServiceProvider();
            IMintCartAppSettings? appSettingsHelper = serviceProviderservicesProvider.GetService<IMintCartAppSettings>();
            ConfigurationResponse<string>? identityUrl = appSettingsHelper?.Get<string>(AppSettingConstants.IdentityUrl, "");
            return identityUrl?.Response ?? "";
        }

        private static string GetIdentityAudience(this IServiceCollection services)
        {
            IServiceProvider serviceProviderservicesProvider = services.BuildServiceProvider();
            IMintCartAppSettings? appSettingsHelper = serviceProviderservicesProvider.GetService<IMintCartAppSettings>();
            ConfigurationResponse<string>? identityAudience = appSettingsHelper?.Get<string>(AppSettingConstants.IdentityAudience, "");
            return identityAudience?.Response ?? "";
        }
        #endregion
    }
}
