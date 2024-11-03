using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.Caching;

namespace AwaraIT.Training.Application.Core
{
    public class Cache
    {
        private const int CacheExpirationSec = 1800;

        private readonly IOrganizationService _service;
        private readonly ObjectCache _cache = MemoryCache.Default;

        public Cache(IOrganizationService service)
        {
            _service = service;
        }



        private T GetOrAddValue<T>(string key, int expirationSec, Func<T> getValue)
        {
            T value;
            if (_cache.Contains(key))
            {
                value = (T)_cache.Get(key);
            }
            else
            {
                value = getValue();
                if (value != null)
                    _cache.Add(key, value, DateTime.Now.AddSeconds(expirationSec));
            }

            return value;
        }

        private T GetOrAddValue<T>(string key, Func<T> getValue)
        {
            return GetOrAddValue(key, CacheExpirationSec, getValue);
        }
    }
}
