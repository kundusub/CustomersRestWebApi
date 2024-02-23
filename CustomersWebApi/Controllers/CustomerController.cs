using CustomersWebApi.DistributedCache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestServerProgram.Model;
using System.Text;

namespace RestServerProgram.Controllers
{
    /// <summary>
    /// CustomerController
    /// </summary>
    public class CustomerController : System.Web.Http.ApiController
    {
        IDistributedCachingService _service;
        
        public CustomerController(IDistributedCachingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add customer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/AddCustomer")]
        public bool AddCustomer([FromBody] List<Customer> customers)
        {
            bool flag = false;
            List<Customer> freshCustomerList = new List<Customer>();
            try
            {
                var CustomerListAbove18 = customers != null ? customers.Where(y => y.Id != 0 &&
                                                                               !string.IsNullOrEmpty(y.Id.ToString()) &&
                                                                               !String.IsNullOrEmpty(y.FirstName) && 
                                                                               !String.IsNullOrEmpty(y.LastName) &&
                                                                               !String.IsNullOrEmpty(y.Age.ToString())
                                                                            )
                                                                        .Where(x => x.Age >= 18)
                                                                            .OrderBy(x => x.LastName).OrderBy(x => x.FirstName).ToList() : null;

                var cachedCustomerSortedList = _service.GetCacheData().OrderBy(x => x.LastName).OrderBy(x => x.FirstName);

                if (cachedCustomerSortedList.Count() > 0)
                {
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
                }
                else
                {
                    foreach (var newCustomer in CustomerListAbove18)
                    {
                        freshCustomerList.Add(newCustomer);
                    }
                }
                flag = true;
            }
            finally
            {
                var strFreshCustomerList = JsonConvert.SerializeObject(freshCustomerList);
                _service.SetCacheData(Encoding.Unicode.GetBytes(strFreshCustomerList));
            }
            return flag;
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Customer/GetCustomer")]
        public List<Customer> GetCustomer()
        {
            List<Customer> customerList = new List<Customer>();
            try
            {
                customerList = _service.GetCacheData().ToList();

            }
            finally { }
            return customerList;
        }
    }
}
