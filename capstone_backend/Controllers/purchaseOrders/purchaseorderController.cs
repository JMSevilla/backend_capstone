using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.purchaseOrders
{
    [RoutePrefix("api/purchase-order")]
    public class purchaseorderController : ApiController
    {
        private local_dbbmEntities core;
        class Response
        {
            public string message { get; set; }
        }
        Response response = new Response();
        [Route("bulk-entry-purchase"), HttpPost]
        public HttpResponseMessage bulkentry(int quantity, int price
            , string ponumber, string pname, string supplier)
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = new local_dbbmEntities())
                {
                    if(string.IsNullOrEmpty(pname)
                        || string.IsNullOrEmpty(supplier))
                    {
                        response.message = "empty";
                        return Request.CreateResponse(HttpStatusCode.OK, response.message);
                    }
                    else
                    {
                        puchase_orders orders = new puchase_orders();
                        orders.ponumber = ponumber;
                        orders.pname = pname;
                        orders.pquantity = quantity;
                        orders.pprice = price;
                        orders.ptotal = price * quantity;
                        orders.pcreator = "Administrator";
                        orders.psupplier = supplier;
                        orders.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.puchase_orders.Add(orders);
                        core.SaveChanges();
                        response.message = "success purchase";
                        return Request.CreateResponse(HttpStatusCode.OK, response.message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            };
        }
        //purchase order single entry api
        [Route("single-entry-purchase-order"), HttpPost]
        public HttpResponseMessage singleentry()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = new local_dbbmEntities())
                {
                    puchase_orders orders = new puchase_orders();
                    orders.ponumber = httprequest.Form["ponumber"];
                    orders.pname = httprequest.Form["pname"];
                    orders.pquantity = Convert.ToInt32(httprequest.Form["pquantity"]);
                    orders.pprice = Convert.ToInt32(httprequest.Form["pprice"]);
                    orders.ptotal = Convert.ToInt32(httprequest.Form["pprice"]) * Convert.ToInt32(httprequest.Form["pquantity"]);
                    orders.psupplier = httprequest.Form["psupplier"];
                    orders.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.puchase_orders.Add(orders);
                    core.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "success single entry");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
