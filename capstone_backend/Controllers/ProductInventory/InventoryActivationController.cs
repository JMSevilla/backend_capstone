using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.ProductInventory
{
    [RoutePrefix("api/activation")]
    public class InventoryActivationController : ApiController
    {
        private local_dbbmEntities1 core;
        [Route("product-activation"), HttpPost]
        public HttpResponseMessage activate(int prodid)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    core.update_product_status(prodid, 1);
                    return Request.CreateResponse(HttpStatusCode.OK, "update success");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-deactivation"), HttpPost]
        public HttpResponseMessage deactivate(int prodid)
        {
            try
            {
                using (core = new local_dbbmEntities1())
                {
                    core.update_product_status(prodid, 0);
                    return Request.CreateResponse(HttpStatusCode.OK, "update success");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
