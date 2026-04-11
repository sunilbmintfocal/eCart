using MintCart.Caching;

namespace MintCart.Common
{
    public static class BackgroundSchedularCaching
    {
        public static (bool isExecuted,string cacheData) IsBackgrackgroundJobExecuted(ICacheManager _cacheManager, string? Key, string localDateTime)
        {
            string WorkerLastRunstr = "";

            WorkerLastRunstr = _cacheManager.Get<string>(Key, () => WorkerLastRunstr);

            if (WorkerLastRunstr != localDateTime)
            {
                _cacheManager.Set(Key, localDateTime);
                return (false, WorkerLastRunstr);
            }
            return (true, WorkerLastRunstr);
        }
    }
}
