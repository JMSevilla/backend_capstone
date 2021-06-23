using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.PurchaseOrder
{
    [RoutePrefix("api/purchase-order")]
    public class purchaseController : ApiController
    {
        private local_dbbmEntities core;
        [Route("bulk-entry"), HttpPost]
        public HttpResponseMessage bulkentrypurchase()
        {
            try
            {
                var httpreq = HttpContext.Current.Request;
              foreach(string s in httpreq.Form)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, s);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("list-purchase-order"), HttpGet]
        public HttpResponseMessage getlistofpurchase()
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var obj = core.puchase_orders.ToList();
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
