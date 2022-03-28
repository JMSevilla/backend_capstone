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
                var obj = apiglobalcon.publico.customer_Orders.ToList();
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
                        }else
                        {
                            return Ok("valid qty");
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
                    if(HTTP.Form["buy1take1Indicator"] == "2")
                    {
                        customer_Orders orders = new customer_Orders();
                        orders.orderName = HTTP.Form["buy1take1Prodname"];
                        orders.orderCode = Guid.NewGuid().ToString();
                        orders.orderBarcode = HTTP.Form["buy1take1ProdCode"];
                        orders.orderPrice = Convert.ToDecimal(HTTP.Form["buy1take1Prodprice"]);
                        orders.orderQuantity = Convert.ToInt32(HTTP.Form["buy1take1Quantity"]);
                        orders.orderCategory = HTTP.Form["buy1take1ProdCategory"];
                        orders.orderTotalPrice = Convert.ToDecimal(HTTP.Form["buy1take1Prodprice"]) * Convert.ToInt32(HTTP.Form["buy1take1Quantity"]);
                        orders.orderImage = HTTP.Form["buy1take1Prodimage"];
                        orders.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM"));
                        orders.orderStatus = "1";
                        orders.discountIsApplied = "0";
                        orders.retainedQty = 2 * Convert.ToInt32(HTTP.Form["buy1take1Quantity"]);
                        orders.indicator = HTTP.Form["buy1take1Indicator"];
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
                        orders.indicator = HTTP.Form["solo_order_indicator"];
                        orders.retainedQty = Convert.ToInt32(HTTP.Form["solo_order_qty"]);
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
                    orders.retainedQty = 6 * Convert.ToInt32(HTTP.Form["bundle_order_qty"]);
                    orders.indicator = HTTP.Form["bundle_indicator"];
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
        [Route("order-decrease-qty-prod"), HttpPut]
        public IHttpActionResult decreaseqty(int orderID, int qty, string code)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var check = core.product_finalization.Where(x => x.id == orderID)
                                                         .FirstOrDefault();
                    var cart = core.customer_Orders.Where(x => x.orderBarcode == code).FirstOrDefault();
                    if (check != null)
                    {
                        //subtract final product quantity
                        check.prodquantity = check.prodquantity - qty;
                        //cart.retainedQty = cart.retainedQty + qty;

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
                        return Ok("success_decrease");
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("order-decrease-qty-solo"), HttpPut]
        public IHttpActionResult decreaseqtysolo(int orderID, int qty, int cartID)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var check = core.product_finalization.Where(x => x.id == orderID)
                                                         .FirstOrDefault();
                    var cart = core.customer_Orders.Where(x => x.orderID == cartID).FirstOrDefault();
                    if (check != null)
                    {
                        //subtract final product quantity
                        check.prodquantity = check.prodquantity - qty;
                        cart.retainedQty = cart.retainedQty + qty;
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
                        return Ok("success_decrease");
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("order-decrease-qty-bundle"), HttpPut]
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
                        int total = qty * origqty;
                        //subtract ingredients quantity
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == check.productCode)
                                                                       .Select(x => x.productInventoryCode)
                                                                       .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity - total;
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

        [Route("order-decrease-qty-buy1take1"), HttpPut]
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
                        int total = qty * origqty;
                        //subtract ingredients quantity
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == check.productCode)
                                                                       .Select(x => x.productInventoryCode)
                                                                       .ToList();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity - total;
                        }

                        core.SaveChanges();
                        return Ok("success_decrease");
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
                using (core = apiglobalcon.publico)
                {
                    var total = core.customer_Orders.Select(x => new
                    {
                        x.orderTotalPrice
                    }).Sum(i => i.orderTotalPrice);
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
                        var getRetainedQuantity = core.customer_Orders.Where(x => x.orderBarcode == code).FirstOrDefault();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity + Convert.ToInt32(getRetainedQuantity.retainedQty);
                        }
                    }
                    else if (statusItem == "buy1take1")
                    {
                        var ingredients = core.product_finalization_raw.Where(x => x.productCreatedCode == product.productCode)
                                                                   .Select(x => x.productInventoryCode)
                                                                   .ToList();
                        var getRetainedQuantity = core.customer_Orders.Where(x => x.orderBarcode == code).FirstOrDefault();
                        foreach (var ingredient in ingredients)
                        {
                            var stock = core.product_inventory.Where(x => x.productCode == ingredient).FirstOrDefault();
                            stock.product_quantity = stock.product_quantity + Convert.ToInt32(getRetainedQuantity.retainedQty);
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
                using(core = apiglobalcon.publico)
                {
                    string intotal = "select count(paymentStatus) from paymentDetails where paymentStatus=1";
                    object total = core.Database.ExecuteSqlCommand(intotal);
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
                        var gettotal = checkID.orderPrice * checkID.orderQuantity;
                        checkID.orderTotalPrice = gettotal;
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
        [Route("update-qty-cart-boxof6"), HttpPut]
        public IHttpActionResult updateBoxof6QTY(int id, int qty)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var checkID = core.customer_Orders.Where(x => x.orderID == id).FirstOrDefault();
                    if (checkID != null)
                    {
                        checkID.orderQuantity = checkID.orderQuantity + qty;
                        checkID.retainedQty = checkID.retainedQty + 6 * qty;
                        var gettotal = checkID.orderPrice * checkID.orderQuantity;
                        checkID.orderTotalPrice = gettotal;
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
        [Route("update-qty-cart-b1t1"), HttpPut]
        public IHttpActionResult updateb1t1QTY(int id, int qty)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var checkID = core.customer_Orders.Where(x => x.orderID == id).FirstOrDefault();
                    if (checkID != null)
                    {
                        checkID.orderQuantity = checkID.orderQuantity + qty;
                        checkID.retainedQty = checkID.retainedQty + 2 * qty;
                        var gettotal = checkID.orderPrice * checkID.orderQuantity;
                        checkID.orderTotalPrice = gettotal;
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
        [Route("sales-entry"), HttpPost]
        public IHttpActionResult salesEntry()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var HTTP = HttpContext.Current.Request;
                    product_sales sales = new product_sales();
                    sales.salesInfo = HTTP.Form["orderInfo"];
                    sales.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    core.product_sales.Add(sales);
                    core.SaveChanges();
                    return Ok("success sales entry");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("ready-payment-deletion"), HttpDelete]
        public IHttpActionResult paymentDetailsDeletion()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    string deleteStatus = "delete from paymentDetails";
                    core.Database.ExecuteSqlCommand(deleteStatus);
                    return Ok("success delete");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("inventory-detection-get-integratedraws"), HttpGet]
        public IHttpActionResult getDetectionInventory(int id, int indicator)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    //b1t1
                    if(indicator == 2)
                    {
                        var checkExceedingQty = core.product_finalization.Where(x => x.id == id)
                            .Select(y => new
                            {
                                y.integratedRaws
                            }).ToList();
                        return Ok(checkExceedingQty);
                    }
                    //box of 6
                    else if(indicator == 3)
                    {
                        var checkExceedingQty = core.product_finalization.Where(x => x.id == id)
                            .Select(y => new
                            {
                                y.integratedRaws
                            }).ToList();
                        return Ok(checkExceedingQty);
                    }
                    // solo
                    else
                    {
                        var checkExceedingQty = core.product_finalization.Where(x => x.id == id)
                            .Select(y => new
                            {
                                y.integratedRaws
                            }).ToList();
                        return Ok(checkExceedingQty);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-product-final-integration"), HttpGet]
        public IHttpActionResult getIntegRaws(string barcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var getRaws = core.product_finalization.Where(x => x.productCode == barcode).Select(y => new
                    {
                        y.integratedRaws
                    }).ToList();
                    return Ok(getRaws);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("check-detection-product-invetory-quantity"), HttpGet]
        public IHttpActionResult detectIfExceed(int quantity, int indicator, string prodcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if (indicator == 3)
                    {
                        var checkBoxOf6 = core.product_inventory.Where(x => x.productCode == prodcode).FirstOrDefault();
                        int result = 6 * quantity; 
                        if (checkBoxOf6.product_quantity < result)
                        {
                            return Ok("exceed box of 6");
                        }
                        else
                        {
                            return Ok("accept box of 6");
                        }
                    }
                    //b1t1
                    else if (indicator == 2)
                    {
                        var checkb1t1 = core.product_inventory.Where(x => x.productCode == prodcode).FirstOrDefault();
                        int result = 2 * quantity;
                        if (checkb1t1.product_quantity < result)
                        {
                            return Ok("exceed b1t1");
                        }
                        else
                        {
                            return Ok("accept b1t1");
                        }
                    }
                    //solo
                    else
                    {
                        var checksolo = core.product_inventory.Where(x => x.productCode == prodcode).FirstOrDefault();
                        if (checksolo.product_quantity < 1)
                        {
                            return Ok("exceed solo");
                        }
                        else
                        {
                            return Ok("accept solo");
                        }
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
