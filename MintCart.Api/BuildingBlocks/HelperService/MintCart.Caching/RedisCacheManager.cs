using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using MintCart.Caching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using MintCart.Caching.Redis;
using Newtonsoft.Json;

namespace MintCart.Caching
{

    public partial class RedisCacheManager : ICacheManager
    {
        #region Fields
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly ILogger<RedisCacheManager> _logger;
        private readonly IDatabase _db;
        #endregion

        #region Ctor

        public RedisCacheManager(IRedisConnectionWrapper connectionWrapper, ILogger<RedisCacheManager> logger,
            RedisConfig config)
        {
            if (string.IsNullOrEmpty(config.ConnectionString))
                throw new Exception("Redis connection string is empty");

            // ConnectionMultiplexer.Connect should only be called once and shared between callers
            _connectionWrapper = connectionWrapper;
            _logger = logger;
            _db = _connectionWrapper.GetDatabase();
        }

        #endregion
        
        #region Utilities

      
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <returns>The cached value associated with the specified key</returns>
        protected virtual async Task<T> GetAsync<T>(string key)
        {
            //_logger.LogDebug($"Redis Cache Manager : Get Async() - Key {key}");
            //get serialized item from cache
            var serializedItem = await _db.StringGetAsync(key);
            //_logger.LogDebug($"Serialized item - {serializedItem}");
            //_logger.LogDebug($"Serialized item HasValue ? - {serializedItem.HasValue}");

            if (!serializedItem.HasValue)
                return default(T);

            //deserialize item
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            //_logger.LogDebug($"Deserialized item - {item}");

            if (item == null)
                return default(T);

            return item;
        }
        
        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            //Mainly this is to avoid exception result from GRPc services
            if (data is string || data is String)
            {
                var isException = data.ToString().Contains("Exception");
                if (isException)
                {
                    _logger.LogDebug($"Caching text contains keyword \"Exception\". Hence avoiding the caching. Key = {key}. Caching text/ERROR - {data}");
                    return;
                }
            }

            //set cache time
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            await _db.StringSetAsync(key, serializedItem, expiresIn);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        protected virtual async Task<bool> IsSetAsync(string key)
        {
            //_logger.LogDebug($"Checking Is Set of key - {key}");
            return await _db.KeyExistsAsync(key);
        }

        public virtual async Task RemoveAsync(string key)
        {
            _logger.LogDebug($"Removing key - {key}");
            //remove item from caches
            await _db.KeyDeleteAsync(key);
        }
        public virtual void Remove(string key)
        {
            _logger.LogDebug($"Removing key - {key}");
            //remove item from caches
             _db.KeyDelete(key);
        }

        /// <summary>
        /// Removes items by key prefix
        /// </summary>
        /// <param name="prefix">String key prefix</param>
        public virtual async Task RemoveByPrefix(string prefix)
        {
            _logger.LogDebug($"RemoveByPrefix(): PREFIX : {prefix}");

            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint, prefix);
                _logger.LogDebug($"RemoveByPrefix(): KEYS : {keys}");

                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }

        public virtual List<string> GetKeysByPrefix(string prefix)
        {
            _logger.LogDebug($"GetKeysByPrefix(): PREFIX : {prefix}");
            List<string> keysList = new List<string>();
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                keysList.AddRange(GetKeys(endPoint, prefix).Select(x => x.ToString()).ToList());
            }
            _logger.LogDebug($"GetKeysByPrefix(): KEYS : {keysList}");
            return keysList;
        }
        public virtual async Task<List<T>> GetAll<T>(List<string> key)
        {
            _logger.LogDebug($"GetAll(): Key : {key}");
            List<T> data = new List<T>();
            var keysList = key.Select(x => (RedisKey)x).ToArray();
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var result = await _db.StringGetAsync(keysList);
                foreach (var item in result)
                    if (item.HasValue)
                    {
                        var _item = JsonConvert.DeserializeObject<T>(item);
                        if (_item != null)
                            data.Add(_item);
                    }
            }
            _logger.LogDebug($"GetAll(): KEYS : {keysList}");
            return data;
        }
        /// <summary>
        /// Gets the list of cache keys prefix
        /// </summary>
        /// <param name="endPoint">Network address</param>
        /// <param name="prefix">String key pattern</param>
        /// <returns>List of cache keys</returns>
        protected virtual IEnumerable<RedisKey> GetKeys(EndPoint endPoint, string prefix = null)
        {
            _logger.LogDebug($"ENDPOINT : {endPoint}");
            _logger.LogDebug($"PREFIX : {prefix}");

            var server = _connectionWrapper.GetServer(endPoint);
            _logger.LogDebug($"SERVER : {server}");
            //we can use the code below (commented), but it requires administration permission - ",allowAdmin=true"
            //server.FlushDatabase();
            var keys = server.Keys(_db.Database, string.IsNullOrEmpty(prefix) ? null : $"{prefix}*");
            _logger.LogDebug($"KEYS : {keys}");

            return keys;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
        {
            //item already is in cache, so return it
            if (await IsSetAsync(key))
            {
                //_logger.LogDebug($"Item already is in cache, so return it. Key - {key}");
                return await GetAsync<T>(key);
            }

            //_logger.LogDebug($"Item not there in cache. Key - {key}");

            //or create it using passed function
            var result = await acquire();

            // set in cache (if cache time is defined)
            if ((cacheTime ?? CachingDefaults.CacheTime) > 0)
                await SetAsync(key, result, cacheTime ?? CachingDefaults.CacheTime);
            
            return result;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <returns>The cached value associated with the specified key</returns>
        public virtual T Get<T>(string key)
        {

            //get serialized item from cache
            var serializedItem = _db.StringGet(key);
            if (!serializedItem.HasValue)
                return default(T);

            //deserialize item
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);

            return item;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            //item already is in cache, so return it
            if (IsSet(key))
            {
                _logger.LogDebug($"Item already is in cache, so return it. Key - {key}");
                return Get<T>(key);
            }
            _logger.LogDebug($"Item not there in cache. Key - {key}");

            //or create it using passed function
            var result = acquire();

            //and set in cache (if cache time is defined)
            if ((cacheTime ?? CachingDefaults.CacheTime) > 0)
                Set(key, result, cacheTime ?? CachingDefaults.CacheTime);

            return result;
        }
        
        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        public virtual void Set(string key, object data, int? cacheTime = null)
        {
            if (data == null)
                return;

            //Mainly this is to avoid exception result from GRPc services
            if (data is string || data is String)
            {
                var isException = data.ToString().Contains("Exception");
                if (isException)
                {
                    _logger.LogDebug($"Caching text contains keyword \"Exception\". Hence avoiding the caching. Key = {key}. Caching text/ERROR - {data}");
                    return;
                }
            }

            //set cache time
            var expiresIn = TimeSpan.FromMinutes(cacheTime ?? CachingDefaults.CacheTime);

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            _db.StringSet(key, serializedItem, expiresIn);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        public virtual bool IsSet(string key)
        {

            return _db.KeyExists(key);
        }

        /// <summary>
        /// Get a cached hash item.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetHashEntries<T>(string key)
        {
            try
            {
                var hashValue = await _db.HashGetAllAsync(key);
                return hashValue.ConvertFromRedis<T>();
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        
        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>

        /// <summary>
        /// Dispose cache manager
        /// </summary>
        public virtual void Dispose()
        {
            if (_connectionWrapper != null)
                _connectionWrapper.Dispose();
        }

        public virtual T GetSetting<T>(string tenantID, Func<T> acquire, int? cacheTime = null)
        {
            string key = $"MintCart.{tenantID}.Settings.{typeof(T).Name}";
            //item already is in cache, so return it
            if (IsSet(key))
            {
                _logger.LogDebug($"Item already is in cache, so return it. Key - {key}");
                return Get<T>(key);
            }
            _logger.LogDebug($"Item not there in cache. Key - {key}");

            //or create it using passed function
            var result = acquire();

            //and set in cache (if cache time is defined)
            if ((cacheTime ?? CachingDefaults.CacheTime) > 0)
                Set(key, result, cacheTime ?? CachingDefaults.CacheTime);

            return result;
        }

        #endregion
    }
    public static class RedisUtils
    {
        /// <summary>
        /// Deserialize from Redis format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashEntries"></param>
        /// <returns></returns>
        public static T ConvertFromRedis<T>(this HashEntry[] hashEntries)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var obj = Activator.CreateInstance(typeof(T));
            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => Regex.Replace(g.Name, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled).ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry())) continue;
                property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), property.PropertyType));
            }
            return (T)obj;
        }
    }
    }
