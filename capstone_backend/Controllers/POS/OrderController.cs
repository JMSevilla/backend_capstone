using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;

namespace capstone_backend.Controllers.POS
{
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        [Route("order-list"), HttpGet]
        public async Task<IHttpActionResult> getorderlist()
        {
            try
            {
                IHttpActionResult result = null;
                var obj = apiglobalcon.publico.customer_Orders.ToList().Distinct();
                result = Ok(obj);
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
