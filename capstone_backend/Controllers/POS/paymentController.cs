using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
using System.Web;
using System.Data.SqlClient;

namespace capstone_backend.Controllers.POS
{
    [RoutePrefix("api/payment")]
    public class paymentController : ApiController
    {
        dbbmEntities core;
        class Response
        {
            public string message { get; set; }
        }
        Response resp = new Response();
        [Route("make-payment"), HttpPost]
        public IHttpActionResult makePayment()
        {
            try
            {
                var HTTP = HttpContext.Current.Request;
                var rawSQL = "insert into paymentDetails values(@paymentInfo, @paymentStatus, @date, @orderinfo)";
                var rawDeletionCart = "delete from customer_Orders";
                using(core = apiglobalcon.publico)
                {
                    core.Database.ExecuteSqlCommand(rawSQL, new SqlParameter("@paymentInfo", HTTP.Form["paymentinfo"]),
                        new SqlParameter("@paymentStatus", "1"), new SqlParameter("@date", Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM"))),
                        new SqlParameter("@orderinfo", HTTP.Form["orderinfo"]));

                    core.Database.ExecuteSqlCommand(rawDeletionCart);
                    resp.message = "success make payment";
                    return Ok(resp);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("confirm-payment/{code}"), HttpGet]
        public IHttpActionResult getIngredientsjson(string code)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.product_finalization.Where(x => x.productCode == code).Select(y => new
                    {
                        y.integratedRaws
                    }).ToList().Distinct();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("decrease-solo/{prodid}/{qty}"), HttpPut]
        public IHttpActionResult postSoloReducer(int prodid, int qty)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var reduceQuery = "update product_inventory set product_quantity=product_quantity-@qty where productID=@prodid";
                    core.Database.ExecuteSqlCommand(reduceQuery, new SqlParameter("@qty", qty), new SqlParameter("@prodid", prodid));
                    return Ok("success reduce");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("approve-payment-update-to-receipt/{paymentID}/{isbundle}"), HttpPut]
        public IHttpActionResult paymentDone(int paymentID, bool isbundle)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if (isbundle)
                    {
                        var donepayment = "update paymentDetails set paymentStatus='3' where paymentID=@pid";
                        core.Database.ExecuteSqlCommand(donepayment, new SqlParameter("@pid", paymentID));
                        return Ok("done payment");
                    }
                    else
                    {
                        var donepayment = "update paymentDetails set paymentStatus='2' where paymentID=@pid";
                        core.Database.ExecuteSqlCommand(donepayment, new SqlParameter("@pid", paymentID));
                        return Ok("done payment");
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
