using Microsoft.Extensions.Caching.Memory;
using System;

namespace PniaApi.Cache
{
    public class CacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly bool _isCacheAllowed;
        private readonly double _expireTimeMinutes;

        public CacheManager(IMemoryCache memoryCache, bool isCacheAllowed, double expireTimeMinutes)
        {
            _cache = memoryCache;
            _isCacheAllowed = isCacheAllowed;
            _expireTimeMinutes = expireTimeMinutes;
        }

        public bool TryGetFromCache<T>(string cacheKey, out T outValue)
        {
            outValue = default;
            if (_isCacheAllowed && _cache.TryGetValue(cacheKey, out outValue))
            {
                return true;
            }
            return false;
        }

        public void SetToCache<T>(string cacheKey, T inValue)
        {
            if (_isCacheAllowed)
            {
                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(_expireTimeMinutes);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                _cache.Set(cacheKey, inValue, cacheExpirationOptions);
            }
        }
    }
}
