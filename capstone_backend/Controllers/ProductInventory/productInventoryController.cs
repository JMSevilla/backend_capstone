using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using capstone_backend.globalCON;
using capstone_backend.Models;
namespace capstone_backend.Controllers.ProductInventory
{
    [RoutePrefix("api/product-inventory")]
    public class productInventoryController : ApiController
    {
        //private local_dbbmEntities1 core;
        private dbbmEntities core;
        [Route("fetchinginventory"), HttpGet]
        public HttpResponseMessage getAllproducts()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var prodobj = core.product_inventory
                       .Where(x => x.product_status == "1").ToList()
                       .OrderByDescending(y => y.productID);
                    return Request.CreateResponse(HttpStatusCode.OK, prodobj);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //fetch all stocks 
        [Route("fetch-stocks-on-hand"), HttpGet]
        public HttpResponseMessage getallstocks()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.stock_on_hand
                        .ToList().OrderByDescending(x => x.stockID);
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //end fetch all stocks
        [Route("fetchinginventory-filter-by-bool"), HttpGet]
        public HttpResponseMessage getproductsbyfilter()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var prodobj = core.product_inventory.ToList()
                      .OrderByDescending(y => y.productID);
                    return Request.CreateResponse(HttpStatusCode.OK, prodobj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //filter search
        [Route("filter-search"), HttpGet]
        public HttpResponseMessage filtersearch(DateTime fromdate, DateTime to)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = (from filterdata in core.product_inventory
                               where (filterdata.createdAt >= fromdate && filterdata.createdAt <= to)
                               orderby filterdata.productID descending
                               select new { 
                               filterdata.productCode,
                               filterdata.productID,
                               filterdata.productName,
                               filterdata.productimgurl,
                               filterdata.product_creator,
                               filterdata.product_price,
                               filterdata.product_quantity,
                               filterdata.product_status,
                               filterdata.product_supplier,
                               filterdata.product_total,
                               filterdata.createdAt
                               }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("more-options"), HttpPost]
        public HttpResponseMessage morefilter()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var httprequest = HttpContext.Current.Request;
                    string code = httprequest.Form["code"];
                    string pname = httprequest.Form["pname"];
                    string pstatus = httprequest.Form["pstatus"];
                    string category = httprequest.Form["filterbycateg"];
                    bool filterbased = Convert.ToBoolean(httprequest.Form["filterbased"]);
                    if (filterbased == false)
                    {
                        var objcode = (from codesearch in core.product_inventory
                                       where (codesearch.productCode == code || codesearch.productName == pname
                                       || codesearch.product_status == pstatus || codesearch.product_category == category)
                                       orderby codesearch.productID descending
                                       select new
                                       {
                                           codesearch.productCode,
                                           codesearch.productID,
                                           codesearch.productName,
                                           codesearch.productimgurl,
                                           codesearch.product_creator,
                                           codesearch.product_price,
                                           codesearch.product_quantity,
                                           codesearch.product_status,
                                           codesearch.product_supplier,
                                           codesearch.product_total,
                                           codesearch.createdAt
                                       }).ToList();
                        return Request.CreateResponse
                            (HttpStatusCode.OK, objcode);
                    }
                    else
                    {
                        var objcode = (from codesearch in core.product_inventory
                                       where (codesearch.productCode == code && codesearch.productName == pname
                                       && codesearch.product_status == pstatus && codesearch.product_category == category)
                                       orderby codesearch.productID descending
                                       select new
                                       {
                                           codesearch.productCode,
                                           codesearch.productID,
                                           codesearch.productName,
                                           codesearch.productimgurl,
                                           codesearch.product_creator,
                                           codesearch.product_price,
                                           codesearch.product_quantity,
                                           codesearch.product_status,
                                           codesearch.product_supplier,
                                           codesearch.product_total,
                                           codesearch.createdAt
                                       }).ToList();
                        return Request.CreateResponse
                            (HttpStatusCode.OK, objcode);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //end filter search
        [Route("adding-product-inventory"), HttpPost]
        public HttpResponseMessage addProductInventory()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using (core = apiglobalcon.publico)
                {
                    if (string.IsNullOrEmpty(httprequest.Form["productCode"]) ||
                        string.IsNullOrEmpty(httprequest.Form["productName"]) ||
                        string.IsNullOrEmpty(httprequest.Form["productQuantity"])
                        || string.IsNullOrEmpty(httprequest.Form["productcategory"])
                        || string.IsNullOrEmpty(httprequest.Form["productImageUrl"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        string syncing = httprequest.Form["productsync"];
                        if(syncing == "1")
                        {
                            if (!string.IsNullOrEmpty(httprequest.Form["productexpiration"]))
                            {
                                stock_on_hand stock = new stock_on_hand();
                                stock.productname = httprequest.Form["productName"];
                                stock.stockNumber = httprequest.Form["productCode"];
                                stock.productquantity = Convert.ToInt32(httprequest.Form["productQuantity"]);
                                stock.productprice = 0;
                                stock.productsupplier = httprequest.Form["productSupplier"];
                                stock.productimgurl = httprequest.Form["productImageUrl"];
                                stock.product_total = 0;
                                stock.productcreator = "Administrator";
                                stock.productstatus = "1";
                                stock.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                stock.productcategory = httprequest.Form["productcategory"];
                                stock.expirationprod = Convert.ToDateTime(httprequest.Form["productexpiration"]);
                                stock.sizes = "None";
                                core.stock_on_hand.Add(stock);
                                core.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, "success product inventory");
                            }
                            else
                            {
                                stock_on_hand stock = new stock_on_hand();
                                stock.productname = httprequest.Form["productName"];
                                stock.stockNumber = httprequest.Form["productCode"];
                                stock.productquantity = Convert.ToInt32(httprequest.Form["productQuantity"]);
                                stock.productprice = 0;
                                stock.productsupplier = httprequest.Form["productSupplier"];
                                stock.productimgurl = httprequest.Form["productImageUrl"];
                                stock.product_total = 0;
                                stock.productcreator = "Administrator";
                                stock.productstatus = "1";
                                stock.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                stock.productcategory = httprequest.Form["productcategory"];
                                stock.sizes = "None";
                                core.stock_on_hand.Add(stock);
                                core.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, "success product inventory");
                            }
                            
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(httprequest.Form["productexpiration"]))
                            {
                                product_inventory prod = new product_inventory();
                                prod.productName = httprequest.Form["productName"];
                                prod.productCode = httprequest.Form["productCode"];
                                prod.product_quantity = Convert.ToInt32(httprequest.Form["productQuantity"]);
                                prod.product_price = 0;
                                prod.product_supplier = httprequest.Form["productSupplier"];
                                prod.productimgurl = httprequest.Form["productImageUrl"];
                                prod.product_total = 0;
                                prod.product_creator = httprequest.Form["isadmin"];
                                prod.product_status = httprequest.Form["isstatus"];
                                prod.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                prod.product_category = httprequest.Form["productcategory"];
                                prod.expirationprod = Convert.ToDateTime(httprequest.Form["productexpiration"]);
                                prod.size = "None";
                                core.product_inventory.Add(prod);
                                core.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, "success product inventory");
                            }
                            else
                            {
                                product_inventory prod = new product_inventory();
                                prod.productName = httprequest.Form["productName"];
                                prod.productCode = httprequest.Form["productCode"];
                                prod.product_quantity = Convert.ToInt32(httprequest.Form["productQuantity"]);
                                prod.product_price = 0;
                                prod.product_supplier = httprequest.Form["productSupplier"];
                                prod.productimgurl = httprequest.Form["productImageUrl"];
                                prod.product_total = 0;
                                prod.product_creator = httprequest.Form["isadmin"];
                                prod.product_status = httprequest.Form["isstatus"];
                                prod.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                prod.product_category = httprequest.Form["productcategory"];
                                prod.size = "None";
                                core.product_inventory.Add(prod);
                                core.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, "success product inventory");
                            }
                                
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-inventory-deletion"), HttpPost]
        public HttpResponseMessage proddeletion(int prodid, string pcode, int pquantity)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var check = core.stock_on_hand.Any(x => x.stockNumber == pcode);
                    if (check)
                    {
                        if (prodid <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid id");
                        }
                        else
                        {
                            core.quantity_decrease_manager(null, pquantity, pcode, 3);
                            var deletion = core.product_inventory.Where(x => x.productID == prodid).FirstOrDefault();
                            core.Entry(deletion).State = System.Data.Entity.EntityState.Deleted;
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success deletion");
                        }
                    }
                    else
                    {
                        if (prodid <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid id");
                        }
                        else
                        {
                            var deletion = core.product_inventory.Where(x => x.productID == prodid).FirstOrDefault();
                            core.Entry(deletion).State = System.Data.Entity.EntityState.Deleted;
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success deletion");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-inventory-modification"), HttpPost]
        public HttpResponseMessage modifyproduct(string tagalert)
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = apiglobalcon.publico)
                {
                    
                    if(string.IsNullOrEmpty(httprequest.Form["modifyproductname"]) || string.IsNullOrEmpty(httprequest.Form["modifyproductimageurl"])
                        || string.IsNullOrEmpty(httprequest.Form["modifyproductquantity"]) || string.IsNullOrEmpty(httprequest.Form["modifyproductprice"])
                        || string.IsNullOrEmpty(httprequest.Form["modifycategory"]) || string.IsNullOrEmpty(httprequest.Form["modifyPID"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        if(tagalert == "1")
                        {
                            core.update_product_inventory
                            (Convert.ToInt32(httprequest.Form["modifyPID"]), httprequest.Form["modifyproductname"],
                            Convert.ToInt32(httprequest.Form["modifyproductquantity"]), 0,
                            Convert.ToInt32(httprequest.Form["modifyproductprice"]) * Convert.ToInt32(httprequest.Form["modifyproductquantity"]),
                            httprequest.Form["modifyproductsupplier"], httprequest.Form["modifyproductimageurl"], httprequest.Form["modifycategory"], httprequest.Form["pcode"] , 2);
                            
                            return Request.CreateResponse
                                (HttpStatusCode.OK, "success modify");
                        }
                        else
                        {
                            core.update_product_inventory
                            (Convert.ToInt32(httprequest.Form["modifyPID"]), httprequest.Form["modifyproductname"],
                            Convert.ToInt32(httprequest.Form["modifyproductquantity"]), 0,
                            Convert.ToInt32(httprequest.Form["modifyproductprice"]) * Convert.ToInt32(httprequest.Form["modifyproductquantity"]),
                            httprequest.Form["modifyproductsupplier"], httprequest.Form["modifyproductimageurl"], httprequest.Form["modifycategory"], null, 1);
                            return Request.CreateResponse
                                (HttpStatusCode.OK, "success modify");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("check-if-exist-in-stocks"), HttpPost]
        public HttpResponseMessage checkstocks(string pcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if(core.stock_on_hand.Any(x => x.stockNumber == pcode))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "exist");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "not exist");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("checking-json-product-inventory"), HttpGet]
        public IHttpActionResult JSONChecker(
            string pcode
            )
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.product_finalization.Where(x => x.productCode == pcode)
                        .Select(y => new
                        {
                            y.integratedRaws
                        }).ToList();
                    return Ok(obj);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("validate-product-inventory"), HttpGet]
        public IHttpActionResult validateInventory(string prodcode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var check = core.product_inventory.Where(x => x.productCode == prodcode).FirstOrDefault();
                    if(check != null)
                    {
                        return Ok("exist");
                    }
                    else
                    {
                        return Ok("not exist");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("deletion-json"), HttpPost]
        public Task<IHttpActionResult> deletionjson(
            int prodid, DateTime created, DateTime exp, string pcode
            )
        {
            using (core = apiglobalcon.publico)
            {
                var HTTP = HttpContext.Current.Request;
                var secondChecker = core.product_finalization.Any(x => x.id == prodid);
                var PIChecker = core.product_inventory.Any(x => x.productCode == pcode);
                IHttpActionResult result = null;
                
                if (secondChecker)
                {
                    if (PIChecker)
                    {
                        result = Ok("pi exist");
                        return Task.FromResult(result);
                    }
                    else
                    {
                        //algo
                        core.deletion_json(
                            prodid, Convert.ToInt32(HTTP.Form["productID"]), HTTP.Form["productCode"],
                            HTTP.Form["productName"], Convert.ToInt32(HTTP.Form["product_quantity"]),
                            Convert.ToDecimal(HTTP.Form["product_price"]), Convert.ToInt32(HTTP.Form["product_total"]),
                            HTTP.Form["product_status"], HTTP.Form["product_creator"], HTTP.Form["product_supplier"],
                            created, HTTP.Form["product_image"], HTTP.Form["product_category"],
                            exp, HTTP.Form["size"], "deletion_ingredients_json"
                            );
                        result = Ok("done");
                        return Task.FromResult(result);

                    }
                }
                else
                {
                    result = Ok("pf not exist");
                    return Task.FromResult(result);
                }
            }
        }
    }
}
