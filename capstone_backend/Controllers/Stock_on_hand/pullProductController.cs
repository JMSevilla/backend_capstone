using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
using capstone_backend.globalCON;
using System.Data.SqlClient;

namespace capstone_backend.Controllers.Stock_on_hand
{
    [RoutePrefix("api/pull-request-product")]
    public class pullProductController : ApiController
    {
        //private burgerdbEntities core;
        private dbbmEntities core;
        [Route("sync-data-to-product-inventory"), HttpPost]
        public HttpResponseMessage syncdata(int id, string pname, string pcode, int pquantity, decimal pprice, string supplier, string category, string expiration)
        {
            try
            {
                using(core = apiglobalcon.publico)
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
                                prod.size = "None";
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
                                prod.size = "None";
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
                using(core = apiglobalcon.publico)
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
                using(core = apiglobalcon.publico)
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
                using(core = apiglobalcon.publico)
                {
                    var obj = core.stock_on_hand.Where(x => x.productstatus == "1").ToList().OrderByDescending(x => x.stockID);
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
                using (core = apiglobalcon.publico)
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
                using(core = apiglobalcon.publico)
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
        [Route("stock-to-archive"), HttpPut]
        public IHttpActionResult stocktoArchive(int stockid)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var check = core.stock_on_hand.Where(x => x.stockID == stockid).FirstOrDefault();
                    if(check != null)
                    {
                        check.productstatus = "3";
                        core.SaveChanges();
                    }
                    return Ok("success");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("refill-entry"), HttpPost]
        public IHttpActionResult refillEntry(
            string dumpprodname, int dumpid, long dumpNumber, int qty, DateTime exp, DateTime received
            )
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                   if(core.stocks_on_hand_dump.Any(x => x.dumpExpiration == exp))
                    {
                        var check = core.stocks_on_hand_dump.Where(x => x.dumpExpiration == exp).FirstOrDefault();
                        if (check.dumpExpiration == exp)
                        {
                            var Updater = core.stocks_on_hand_dump.Where(x => x.dumpExpiration == exp).FirstOrDefault();
                            if(Updater!= null)
                            {
                                Updater.dumpQuantity = Updater.dumpQuantity + qty;
                                core.SaveChanges();
                            }
                            return Ok("success refill update");
                        }
                        else
                        {
                            stocks_on_hand_dump stonk = new stocks_on_hand_dump();
                            stonk.stockDumpName = dumpprodname;
                            stonk.stockDumpId = dumpid;
                            stonk.stockDumpNumber = dumpNumber;
                            stonk.dumpQuantity = qty;
                            stonk.dumpExpiration = exp;
                            stonk.dumpReceived = received;
                            stonk.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                            core.stocks_on_hand_dump.Add(stonk);
                            core.SaveChanges();
                            return Ok("success refill");
                        }
                    }
                    else
                    {
                        stocks_on_hand_dump stonk = new stocks_on_hand_dump();
                        stonk.stockDumpName = dumpprodname;
                        stonk.stockDumpId = dumpid;
                        stonk.stockDumpNumber = dumpNumber;
                        stonk.dumpQuantity = qty;
                        stonk.dumpExpiration = exp;
                        stonk.dumpReceived = received;
                        stonk.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                        core.stocks_on_hand_dump.Add(stonk);
                        core.SaveChanges();
                        return Ok("success refill");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("refill-list"), HttpGet]
        public IHttpActionResult getRefillList(int stockDumpId)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.stocks_on_hand_dump.Where(x => x.stockDumpId == stockDumpId).ToList();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("stocks-refill"), HttpPut]
        public IHttpActionResult stocksRefill(int dumpid, DateTime currExp, int qty, int genid)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var getExp = core.stock_on_hand.Where(x => x.stockID == dumpid).FirstOrDefault();
                    var helper = core.stock_on_hand.Where(y => y.stockID == dumpid).FirstOrDefault();
                    var deletionGuide = core.stocks_on_hand_dump.Where(a => a.dumpId == dumpid).FirstOrDefault();
                    if(getExp.expirationprod > currExp)
                    {
                        return Ok("invalid pull");
                    }
                    else
                    {
                        if(helper != null)
                        {
                            helper.productquantity = helper.productquantity + qty;
                            helper.expirationprod = currExp;
                            core.SaveChanges();

                            // deletion of stocks refill product
                            string deleteStatus = "delete from stocks_on_hand_dump where dumpId=@id";
                            core.Database.ExecuteSqlCommand(deleteStatus, new SqlParameter("@id", genid));
                        }
                        return Ok("success refill");
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
