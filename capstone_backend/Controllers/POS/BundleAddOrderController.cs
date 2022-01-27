using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
using System.Web;
namespace capstone_backend.Controllers.POS
{
    [RoutePrefix("api/bundle-order")]
    public class BundleAddOrderController : ApiController
    {
        dbbmEntities core;
        [Route("add-to-cart"), HttpPost]
        public IHttpActionResult bundlecartadding()
        {
            var HTTP = HttpContext.Current.Request;
            customer_Orders cart = new customer_Orders();
            try
            {
                using(core = apiglobalcon.publico)
                {
                    cart.orderCode = Guid.NewGuid().ToString();
                    cart.orderName = HTTP.Form["ordername"];
                    cart.orderBarcode = Guid.NewGuid().ToString();
                    cart.orderPrice = Convert.ToDecimal(HTTP.Form["orderprice"]);
                    cart.orderQuantity = 1;
                    cart.orderCategory = "bundle";
                    cart.orderTotalPrice = 0;
                    cart.orderImage = HTTP.Form["orderimage"];
                    cart.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM"));
                    core.customer_Orders.Add(cart);
                    core.SaveChanges();
                    return Ok("success cart");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
