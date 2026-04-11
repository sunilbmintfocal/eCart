using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Validator
{
    public static class ServiceCollectionExtension
    {
        public static void AddValidationHelper(this IServiceCollection services)
        {
            services.AddTransient<List<ValidationError>>();
            services.AddSingleton<ValidatorHelper>();
        }
    }
}
