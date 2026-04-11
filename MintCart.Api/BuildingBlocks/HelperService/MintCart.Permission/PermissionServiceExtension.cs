using Microsoft.Extensions.DependencyInjection;

namespace MintCart.Permission
{
    public static class PermissionServiceExtension
    {
        public static void ConfigurePermission(this IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>();
        }
    }
}
