using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
namespace capstone_backend.Controllers.ExpirationProduct
{
    [RoutePrefix("api/product-invetory-view-expired")]
    public class p_inventory_view_expirationController : ApiController
    {
        //private local_dbbmEntities core;
        private local_dbbmEntities2 core;
        [Route("view-expiration"), HttpGet]
        public HttpResponseMessage viewexpired(string pcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.product_inventory.Where(x => x.productCode == pcode).FirstOrDefault().expirationprod;
                    if (string.IsNullOrEmpty(Convert.ToString(obj)))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "no expiration");
                    }
                    else
                    {
                        var obj1 = core.product_inventory.Where(x => x.productCode == pcode).Select(y => new {
                        y.expirationprod
                        }).ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, obj1);
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
