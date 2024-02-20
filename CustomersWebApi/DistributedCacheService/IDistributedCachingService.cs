using Microsoft.Extensions.Caching.Distributed;
using RestServerProgram.Model;

namespace CustomersWebApi.DistributedCache
{
    public interface IDistributedCachingService
    {
        void SetCacheData(byte[] cache);
        Customer[] GetCacheData();
    }
}
