using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.ExpirationProduct
{
    [RoutePrefix("api/product-invetory-view-expired")]
    public class p_inventory_view_expirationController : ApiController
    {
        private local_dbbmEntities1 core;
        [Route("view-expiration"), HttpGet]
        public HttpResponseMessage viewexpired(string pcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = (from a in core.expirations
                               where a.pcode == pcode
                               select new
                               {
                                   a.expirydate
                               }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
