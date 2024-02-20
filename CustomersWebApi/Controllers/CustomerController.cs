using CustomersWebApi.DistributedCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RestServerProgram.Model;
using System.Text;

namespace RestServerProgram.Controllers
{
    public class CustomerController : System.Web.Http.ApiController
    {
        IDistributedCache _distributedCache;
        IDistributedCachingService _service;
        public CustomerController(IDistributedCache distributedCache, IDistributedCachingService service)
        {
            _distributedCache = distributedCache;
            _service = service;
        }
        [HttpPost]
        public bool AddCustomer([FromBody] string customers)
        {
            bool flag = false;
            List<Customer> freshCustomerList = new List<Customer>();
            try
            {
                var newCustomerList = JsonConvert.DeserializeObject<List<Customer>>(customers);
                var CustomerListAbove18 = newCustomerList != null ? newCustomerList.Where(x => x.Age > 18)
                    .OrderBy(x => x.LastName).OrderBy(x => x.FirstName).ToList() : null;
                var cachedCustomerSortedList = _service.GetCacheData().OrderBy(x => x.LastName).OrderBy(x => x.FirstName);
              

                foreach (var cachedCustomer in cachedCustomerSortedList)
                {
                    bool idMatched = false;
                    freshCustomerList.Add(cachedCustomer);
                    foreach (var newCustomer in CustomerListAbove18)
                    {
                        if (cachedCustomer.Id == newCustomer.Id)
                            break;

                        freshCustomerList.Add(newCustomer);
                    }
                }
                flag = true;
            }
            finally {
                var strFreshCustomerList = JsonConvert.SerializeObject(freshCustomerList);
                _service.SetCacheData(Encoding.Unicode.GetBytes(strFreshCustomerList));
            }
            return flag;
        }
            
    }
}
