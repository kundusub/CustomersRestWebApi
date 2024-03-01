using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RestServerProgram.Model;
using System;
using System.Text;

namespace CustomersWebApi.DistributedCache
{
    public class DistributedCachingService: IDistributedCachingService
    {
        IDistributedCache _distributedCache;
        public DistributedCachingService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public void SetCacheData(byte[] cache)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMonths(1)
            };
            _distributedCache.Set("Customers", cache, cacheOptions);
        }
        public Customer[] GetCacheData()
        {
            Customer[] result = new Customer[0];
            if (_distributedCache.Get("Customers") != null)
            {
                var bytesAsString = Encoding.UTF8.GetString(_distributedCache.GetAsync("Customers").Result);
                result = JsonConvert.DeserializeObject<Customer[]>(bytesAsString);
            }
            return result;

        }
    }
}
