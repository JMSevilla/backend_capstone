using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.ExpirationProduct
{
    [RoutePrefix("api/expired-notif")]
    public class productExpirationController : ApiController
    {
        //private dbbmEntities core; //localhost
        private burgerdbEntities core; //azure cloud computing
        [Route("product-expired"), HttpGet]
        public HttpResponseMessage getexpired()
        {
            try
            {
                using (core = new burgerdbEntities()) //localhost
                {
                    DateTime curdate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    var obj1 = core.product_inventory.Any(x => x.expirationprod <= curdate);
                    if (obj1)
                    {
                        //exist expiration
                        var obj2 = core.product_inventory.Where(x => x.expirationprod <= curdate).ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, obj2);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "not exist expiry");
                    }
                }
               
              
                /* 
                 if(jastine == "jastine") else //
                 */
            }
            catch (Exception)
            {

                throw;
            }
        }
        class ExpirationResponse
        {
            public object respObj { get; set; }
            public string message { get; set; }
        }
        ExpirationResponse respo = new ExpirationResponse();
        [Route("check-10-days-before-expiration"), HttpGet]
        public HttpResponseMessage checkbeforeexpired()
        {
            try
            {
                using (core = new burgerdbEntities())
                {
                    if (core != null)
                    {
                        var obj = core.warning_expiration_10_days.ToList();
                        respo.respObj = obj;
                        respo.message = "will expire after 10 days";
                        return Request.CreateResponse(HttpStatusCode.OK, respo);
                    }
                    else
                    {
                        using (core = new burgerdbEntities())
                        {
                            var obj = core.warning_expiration_10_days.ToList();
                            respo.respObj = obj;
                            respo.message = "will expire after 10 days";
                            return Request.CreateResponse(HttpStatusCode.OK, respo);
                        }
                    }
                }
                
               
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
