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
        private local_dbbmEntities core; //localhost
        private dbbmEntities core1; //azure cloud computing
        [Route("product-expired"), HttpGet]
        public HttpResponseMessage getexpired()
        {
            try
            {
                using (core = new local_dbbmEntities()) //localhost
                {
                    if (core != null)
                    {
                        //code below is for exact date expiration
                        DateTime curdate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                        var obj1 = core.expirations.Any(x => x.expirydate <= curdate);
                        if (obj1)
                        {
                            //exist expiration
                            var obj2 = core.expirations.Where(x => x.expirydate <= curdate).FirstOrDefault().pcode;
                            var obj = core.product_inventory.Where(x => x.productCode == obj2).ToList();
                            return Request.CreateResponse(HttpStatusCode.OK, obj);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "not exist expiry");
                        }
                        //var obj = core.product_inventory.Where(x => x.productCode == obj1).ToList();
                    }

                    else
                    {
                        using (core1 = new dbbmEntities()) //cloud
                        {

                            //code below is for exact date expiration
                            DateTime curdate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                            var obj1 = core1.expirations.Any(x => x.expirydate <= curdate);
                            if (obj1)
                            {
                                //exist expiration
                                var obj2 = core1.expirations.Where(x => x.expirydate <= curdate).FirstOrDefault().pcode;
                                var obj = core1.product_inventory.Where(x => x.productCode == obj2).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, obj);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "not exist expiry");
                            }
                            //var obj = core.product_inventory.Where(x => x.productCode == obj1).ToList();

                        }
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
                using (core = new local_dbbmEntities())
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
                        using (core1 = new dbbmEntities())
                        {
                            var obj = core1.warning_expiration_10_days.ToList();
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
