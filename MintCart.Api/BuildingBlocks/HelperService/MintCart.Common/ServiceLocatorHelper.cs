using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MintCart.Common
{
    public class ServiceLocatorHelper
    {
        private static IServiceProvider _serviceProvider = default!;
        private IServiceProvider _currentServiceProvider;

        public ServiceLocatorHelper(IServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ServiceLocatorHelper Current
        {
            get { return new ServiceLocatorHelper(_serviceProvider); }
        }

        public static void SetServiceLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object? GetInstance(Type serviceType)
        {
            try
            {
                return _currentServiceProvider?.GetService(serviceType);
            }
            catch (Exception) { return null; }
        }

        public TService GetInstance<TService>()
        {
            return _currentServiceProvider.GetService<TService>()??default;
        }
    }
}
