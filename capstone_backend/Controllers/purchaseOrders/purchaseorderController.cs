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
                        orders.status = "0";
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
                    orders.pcreator = "Administrator";
                    orders.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    orders.status = "0";
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
        [Route("received-order"), HttpPost]
        public HttpResponseMessage receivedorder(int id, string pcode, DateTime expiry)
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using (core = new local_dbbmEntities())
                {
                    expiration expire = new expiration();
                    expire.expirydate = expiry;
                    expire.pcode = pcode;
                    expire.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.expirations.Add(expire);
                    core.SaveChanges();

                    product_inventory prod = new product_inventory();
                    prod.productCode = httprequest.Form["pcode"];
                    prod.productName = httprequest.Form["pname"];
                    prod.product_quantity = Convert.ToInt32(httprequest.Form["pquantity"]);
                    prod.product_price = Convert.ToInt32(httprequest.Form["pprice"]);
                    prod.product_total = Convert.ToInt32(httprequest.Form["pprice"]) * Convert.ToInt32(httprequest.Form["pquantity"]);
                    prod.product_status = "0";
                    prod.product_creator = "1";
                    prod.product_supplier = httprequest.Form["psupplier"];
                    prod.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    prod.productimgurl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1200px-No-Image-Placeholder.svg.png";
                    prod.product_category = "";
                    core.product_inventory.Add(prod);
                    core.SaveChanges();

                    core.stored_update_purchase_status(id, 1);
                    return Request.CreateResponse(HttpStatusCode.OK, "success receive");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-supplier-image"), HttpGet]
        public HttpResponseMessage getimage(string supplier)
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var obj = core.suppliers.Where(x => x.supplierfirstname == supplier).ToList();
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
