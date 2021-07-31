using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.Stock_on_hand
{
    [RoutePrefix("api/pull-request-product")]
    public class pullProductController : ApiController
    {
        //private burgerdbEntities core;
        private burgerdbEntities core;
        [Route("sync-data-to-product-inventory"), HttpPost]
        public HttpResponseMessage syncdata(int id, string pname, string pcode, int pquantity, decimal pprice, string supplier, string category, string expiration)
        {
            try
            {
                using(core = new burgerdbEntities())
                {
                    var req = HttpContext.Current.Request;
                    var check = core.product_inventory.Any(x => x.productCode == pcode);
                    if (check)
                    {
                        if (pquantity <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "empty quantity");
                        }
                        else
                        {
                            core.quantity_decrease_manager(id, pquantity, pcode, 2);
                            return Request.CreateResponse(HttpStatusCode.OK, "pull success");
                        }
                    }
                    else
                    {
                        if (pquantity <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "empty quantity");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(expiration) || expiration == "null" || expiration == null)
                            {
                                product_inventory prod = new product_inventory();
                                prod.productName = pname;
                                prod.productCode = pcode;
                                prod.product_quantity = pquantity;
                                prod.product_price = pprice;
                                prod.product_supplier = supplier;
                                prod.productimgurl = req.Form["prodimg"];
                                prod.product_total = pprice * pquantity;
                                prod.product_creator = "1";
                                prod.product_status = "1";
                                prod.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                prod.product_category = category;
                                core.product_inventory.Add(prod);
                                core.SaveChanges();

                                core.quantity_decrease_manager(id, pquantity, null, 1);
                                return Request.CreateResponse(HttpStatusCode.OK, "pull success");
                            }
                            else
                            {

                                product_inventory prod = new product_inventory();
                                prod.productName = pname;
                                prod.productCode = pcode;
                                prod.product_quantity = pquantity;
                                prod.product_price = pprice;
                                prod.product_supplier = supplier;
                                prod.productimgurl = req.Form["prodimg"];
                                prod.product_total = pprice * pquantity;
                                prod.product_creator = "1";
                                prod.product_status = "1";
                                prod.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                prod.product_category = category;
                                prod.expirationprod = Convert.ToDateTime(expiration);
                                core.product_inventory.Add(prod);
                                core.SaveChanges();

                                core.quantity_decrease_manager(id, pquantity, null, 1);
                                return Request.CreateResponse(HttpStatusCode.OK, "pull success");


                                
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("check-stock-quantity-notification"), HttpGet]
        public HttpResponseMessage checkquantitystocks()
        {
            try
            {
                using(core = new burgerdbEntities())
                {
                    var check = core.stock_on_hand.Any(x => x.productquantity <= 0);
                    if (check)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty quantity");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "not empty");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-product-with-zero-quantity"), HttpGet]
        public HttpResponseMessage getzero()
        {
            try
            {
                using(core = new burgerdbEntities())
                {
                    var obj = core.stock_on_hand.Where(x => x.productquantity <= 0).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-all-stocks-on-hand"), HttpGet]
        public HttpResponseMessage getallstocks()
        {
            try
            {
                using(core = new burgerdbEntities())
                {
                    var obj = core.stock_on_hand.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("refill-increase-quantity-by-dropdown"), HttpPost]
        public HttpResponseMessage quantityrefill( int quantity, int id)
        {
            try
            {
                using (core = new burgerdbEntities())
                {
                    if (quantity <= 0 || id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid");
                    }
                    else
                    {
                        core.quantity_refill_increase_manager(id, quantity, 2);
                        return Request.CreateResponse(HttpStatusCode.OK, "success refill");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("remove-zero-quantity-stocks"), HttpPost]
        public HttpResponseMessage remotezeroquantity(int id)
        {
            try
            {
                using(core = new burgerdbEntities())
                {
                    if(id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                    }
                    else
                    {
                        var remover = core.stock_on_hand.Where(x => x.stockID == id).FirstOrDefault();
                        core.Entry(remover).State = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success remove");
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
