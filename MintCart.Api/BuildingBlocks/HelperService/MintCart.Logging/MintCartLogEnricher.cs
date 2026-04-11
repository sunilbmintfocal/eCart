
using MintCart.Common;
using Serilog.Core;
using Serilog.Events;
using MintCart.Identity;


namespace MintCart.Logging
{
	public  class MintCartLogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            IUserContext? _userContext = (IUserContext?)ServiceLocatorHelper.Current.GetInstance(typeof(IUserContext));
            if (_userContext != null)
            {
                var property = propertyFactory.CreateProperty("UserProfileId", _userContext?.UserIdentity ?? string.Empty);

                logEvent.AddOrUpdateProperty(property);
            }
        }
    }
}
