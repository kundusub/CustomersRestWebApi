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
            var bytesAsString = Encoding.UTF8.GetString(_distributedCache.Get("Customers"));
            return JsonConvert.DeserializeObject<Customer[]>(bytesAsString);

        }
    }
}
