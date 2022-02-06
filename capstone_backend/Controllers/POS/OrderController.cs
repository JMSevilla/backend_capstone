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
        dbbmEntities core;
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
        [Route("get-prod-finalization-price"), HttpGet]
        public IHttpActionResult getprodprice(string barcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var getprodprice = core.product_finalization.Where(x => x.productCode == barcode).Select(y => new
                    {
                        y.prodprice
                    }).ToList();
                    return Ok(getprodprice);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-discount"), HttpPut]
        public IHttpActionResult updateDiscountStatus(int id, int newAmount)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var checkSomething = core.customer_Orders.Where(x => x.discountIsApplied == "1").FirstOrDefault();
                    if(checkSomething != null)
                    {
                        return Ok("exist discount");
                    }
                    else
                    {
                        string updateStatus = "update customer_Orders set discountIsApplied=@discount, orderPrice=@orderprice, orderTotalPrice=@ordertotal where orderID=@id";
                        core.Database.ExecuteSqlCommand(updateStatus, new SqlParameter("@discount", "1"), new SqlParameter("@id", id), new SqlParameter("@orderprice", newAmount),
                            new SqlParameter("@ordertotal", newAmount));
                        return Ok("success discount");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("check-update-discount"), HttpGet]
        public IHttpActionResult CheckupdateDiscountStatus()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var checkSomething = core.customer_Orders.Where(x => x.discountIsApplied == "1").FirstOrDefault();
                    if (checkSomething != null)
                    {
                        return Ok("exist discount");
                    }
                    else
                    {
                        
                        return Ok("not exist discount");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("cancel-discount"), HttpPut]
        public IHttpActionResult OnCancelDiscount(int id, string barcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var checkOrder = core.customer_Orders.Where(x => x.orderID == id).FirstOrDefault();
                    if (checkOrder != null)
                    {
                        checkOrder.discountIsApplied = "0";
                        var getOriginalProductPrice = core.product_finalization.Where(x => x.productCode == barcode).FirstOrDefault();
                        string retrieveOriginalPrice = "update customer_Orders set orderPrice=@orderprice, orderTotalPrice=@ordertotal where orderID=@id";
                        core.Database.ExecuteSqlCommand(retrieveOriginalPrice, new SqlParameter("@orderprice", getOriginalProductPrice.prodprice), new SqlParameter("@ordertotal", getOriginalProductPrice.prodprice),
                            new SqlParameter("@id", id));
                        core.SaveChanges();
                        return Ok("success cancellation");
                    }
                    return Ok();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("bundle-validate-cart"), HttpGet]
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
                    if(HTTP.Form["isstatus"] == "buy1take1")
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
                        orders.orderStatus = "1";
                        orders.discountIsApplied = "0";
                        core.customer_Orders.Add(orders);
                        core.SaveChanges();
                        return Ok("success order");
                    } 
                    else 
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
                        orders.orderStatus = "2";
                        orders.discountIsApplied = "0";
                        core.customer_Orders.Add(orders);
                        core.SaveChanges();
                        return Ok("success order");
                    }
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
                    orders.orderStatus = "3";
                    orders.discountIsApplied = "0";
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
        [Route("order-decrease-qty-bundle/{orderID}/{qty}/{origqty}"), HttpPut]
        public IHttpActionResult decreaseqtybundle(int orderID, int qty, int origqty)
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
                        check.prodquantity = check.prodquantity - origqty;
                        
                        //subtract ingredients quantity
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == check.productCode)
                                                                       .Select(x => x.productInventoryCode)
                                                                       .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity - qty;
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

        [Route("order-decrease-qty-buy1take1/{orderID}/{qty}/{origqty}"), HttpPut]
        public IHttpActionResult decreaseqtybuy1take1(int orderID, int qty, int origqty)
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
                        check.prodquantity = check.prodquantity - origqty;

                        //subtract ingredients quantity
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == check.productCode)
                                                                       .Select(x => x.productInventoryCode)
                                                                       .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity - qty;
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
                    string query = "select sum(orderTotalPrice) * orderQuantity from customer_Orders group by orderQuantity";
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
        [Route("void-item/{code}/{qty}/orderno/{orderid}/{statusItem}"), HttpPut]
        public IHttpActionResult voidItem(string code, int qty, int orderid, string statusItem)
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
                    if(statusItem == "boxof6")
                    {
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == product.productCode)
                                                                   .Select(x => x.productInventoryCode)
                                                                   .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity + 6;
                        }
                    }
                    else if (statusItem == "buy1take1")
                    {
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == product.productCode)
                                                                   .Select(x => x.productInventoryCode)
                                                                   .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity + 2;
                        }
                    }
                    else
                    {
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == product.productCode)
                                                                   .Select(x => x.productInventoryCode)
                                                                   .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity + qty;
                        }
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
        [Route("update-qty-cart"), HttpPut]
        public IHttpActionResult updateQTY(int id, int qty)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var checkID = core.customer_Orders.Where(x => x.orderID == id).FirstOrDefault();
                    if(checkID != null)
                    {
                        checkID.orderQuantity = checkID.orderQuantity + qty;
                        core.SaveChanges();
                        return Ok("success update qty");
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
