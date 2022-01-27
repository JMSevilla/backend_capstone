using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace capstone_backend.Controllers.POS
{
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        local_dbbmEntities2 core;
        [Route("order-list"), HttpGet]
        public async Task<IHttpActionResult> getorderlist()
        {
            try
            {
                IHttpActionResult result = null;
                var obj = apiglobalcon.publico.customer_Orders.ToList().Distinct();
                result = Ok(obj);
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("solo-validate-cart/{qty}/{id}"), HttpGet]
        public IHttpActionResult validateSolo(int qty, int id)
        {
            try
            {
                using (apiglobalcon.publico)
                {
                    var check = apiglobalcon.publico.product_finalization
                        .Where(x => x.id == id).FirstOrDefault();
                    if(check != null)
                    {
                        var getvalid = check.prodquantity;
                        if(qty > getvalid)
                        {
                            return Ok("invalid qty");
                        }
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("bundle-validate-cart/{qty}/{id}"), HttpGet]
        public IHttpActionResult validateBundle(int qty, int id)
        {
            try
            {
                using (apiglobalcon.publico)
                {
                    var check = apiglobalcon.publico.product_finalization
                        .Where(x => x.id == id).FirstOrDefault();
                    if (check != null)
                    {
                        var getvalid = check.prodquantity;
                        if (qty > getvalid)
                        {
                            return Ok("invalid qty");
                        }
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("order-solo"), HttpPost]
        public IHttpActionResult OrderSolo()
        {
            try
            {
                var HTTP = HttpContext.Current.Request;
                using (core = apiglobalcon.publico)
                {
                    customer_Orders orders = new customer_Orders();
                    orders.orderName = HTTP.Form["solo_order_name"];
                    orders.orderCode = Guid.NewGuid().ToString();
                    orders.orderBarcode = HTTP.Form["solo_order_code"];
                    orders.orderPrice = Convert.ToDecimal(HTTP.Form["solo_order_price"]);
                    orders.orderQuantity = Convert.ToInt32(HTTP.Form["solo_order_qty"]);
                    orders.orderCategory = HTTP.Form["solo_order_category"];
                    orders.orderTotalPrice = Convert.ToDecimal(HTTP.Form["solo_order_price"]) * Convert.ToInt32(HTTP.Form["solo_order_qty"]);
                    orders.orderImage = HTTP.Form["solo_order_image"];
                    orders.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM"));
                    core.customer_Orders.Add(orders);
                    core.SaveChanges();
                    return Ok("success order");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("order-bundle"), HttpPost]
        public IHttpActionResult OrderBundle()
        {
            try
            {
                var HTTP = HttpContext.Current.Request;
                using (core = apiglobalcon.publico)
                {
                    customer_Orders orders = new customer_Orders();
                    orders.orderName = HTTP.Form["bundle_order_name"];
                    orders.orderCode = Guid.NewGuid().ToString();
                    orders.orderBarcode = HTTP.Form["bundle_order_code"];
                    orders.orderPrice = Convert.ToDecimal(HTTP.Form["bundle_order_price"]);
                    orders.orderQuantity = Convert.ToInt32(HTTP.Form["bundle_order_qty"]);
                    orders.orderCategory = HTTP.Form["bundle_order_category"];
                    orders.orderTotalPrice = Convert.ToDecimal(HTTP.Form["bundle_order_price"]) * Convert.ToInt32(HTTP.Form["bundle_order_qty"]);
                    orders.orderImage = HTTP.Form["bundle_order_image"];
                    orders.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM"));
                    core.customer_Orders.Add(orders);
                    core.SaveChanges();
                    return Ok("success order");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("order-decrease-qty/{orderID}/{qty}"), HttpPut]
        public IHttpActionResult decreaseqty(int orderID, int qty)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var check = core.product_finalization.Where(x => x.id == orderID).FirstOrDefault();
                    if(check != null)
                    {
                        check.prodquantity = check.prodquantity - qty;
                        core.SaveChanges();
                        return Ok("success decrease");
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("order-decrease-qty-bundle/{orderID}/{qty}"), HttpPut]
        public IHttpActionResult decreaseqtybundle(int orderID, int qty)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var check = core.product_finalization.Where(x => x.id == orderID)
                                                         .FirstOrDefault();
                    if (check != null)
                    {
                        //subtract final product quantity
                        check.prodquantity = check.prodquantity - qty;
                        
                        //subtract ingredients quantity
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == check.productCode)
                                                                       .Select(x => x.productInventoryCode)
                                                                       .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.stock_on_hand.Where(x => x.stockNumber == ingredient).FirstOrDefault();
                            stock.productquantity = stock.productquantity - qty;
                        }

                        core.SaveChanges();
                        return Ok("success decrease");
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        SqlConnection cons;
        [Route("order-total-price"), HttpGet]
        public IHttpActionResult gettotal()
        {
            try
            {
                using(cons = apiglobalcon._publiccon)
                {
                    string query = "select sum(orderTotalPrice) from customer_Orders";
                    SqlCommand command = new SqlCommand(query, cons);
                    cons.Open();
                    object total = command.ExecuteScalar();
                    cons.Close();
                    return Ok(total);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("void-item/{code}/{qty}/orderno/{orderid}"), HttpPut]
        public IHttpActionResult voidItem(string code, int qty, int orderid)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                     //add voided quantity to product
                    var product = core.product_finalization.Where(x => x.productCode == code)
                                                           .FirstOrDefault();
                    product.prodquantity = product.prodquantity + qty;
                        
                    //add voided quantity to ingredients
                    var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == product.productCode)
                                                                   .Select(x => x.productInventoryCode)
                                                                   .ToList();
                    foreach (var ingredient in ingredients)
                    {
                        var stock = core.stock_on_hand.Where(x => x.stockNumber == ingredient).FirstOrDefault();
                        stock.productquantity = stock.productquantity + qty;
                    }

                    core.SaveChanges();

                    string voidDeletion = "delete from customer_Orders where orderID=@ordid";
                    core.Database.ExecuteSqlCommand(voidDeletion, new SqlParameter("@ordid", orderid));
                    return Ok("success void");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("count-ready"), HttpGet]
        public IHttpActionResult countready()
        {
            try
            {
                using(cons = apiglobalcon._publiccon)
                {
                    string countquery = "select count(paymentStatus) from paymentDetails where paymentStatus=1";
                    SqlCommand command = new SqlCommand(countquery, cons);
                    cons.Open();
                    object total = command.ExecuteScalar();
                    cons.Close();
                    return Ok(total);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("break-ready-payments"), HttpGet]
        public async Task<IHttpActionResult> getallpayments()
        {
            try
            {
                IHttpActionResult result = null;
                using(core = apiglobalcon.publico)
                {
                    var obj = core.paymentDetails.Where(x => x.paymentStatus == "1").ToList();
                    result = Ok(obj);
                    return await Task.FromResult(result);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
