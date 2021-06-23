using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.ProductInventory
{
    [RoutePrefix("api/product-inventory")]
    public class productInventoryController : ApiController
    {
        private local_dbbmEntities core;
        [Route("fetchinginventory"), HttpGet]
        public HttpResponseMessage getAllproducts()
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var prodobj = core.product_inventory
                       .Where(x => x.product_status == "0").ToList()
                       .OrderByDescending(y => y.productID);
                    return Request.CreateResponse(HttpStatusCode.OK, prodobj);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("fetchinginventory-filter-by-bool"), HttpGet]
        public HttpResponseMessage getproductsbyfilter(bool filter)
        {
            try
            {
                using (core = new local_dbbmEntities())
                {
                    if(filter == true)
                    {
                        var prodobj = core.product_inventory
                       .Where(x => x.product_status == "1").ToList()
                       .OrderByDescending(y => y.productID);
                        return Request.CreateResponse(HttpStatusCode.OK, prodobj);
                    }
                    else
                    {
                        var prodobj = core.product_inventory
                       .Where(x => x.product_status == "0").ToList()
                       .OrderByDescending(y => y.productID);
                        return Request.CreateResponse(HttpStatusCode.OK, prodobj);
                    }
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
                using(core = new local_dbbmEntities())
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
                using(core = new local_dbbmEntities())
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
                using (core = new local_dbbmEntities())
                {
                    if (string.IsNullOrEmpty(httprequest.Form["productCode"]) ||
                        string.IsNullOrEmpty(httprequest.Form["productName"]) ||
                        string.IsNullOrEmpty(httprequest.Form["productQuantity"])
                        || string.IsNullOrEmpty(httprequest.Form["productPrice"])
                        || string.IsNullOrEmpty(httprequest.Form["productcategory"])
                        || string.IsNullOrEmpty(httprequest.Form["productImageUrl"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        product_inventory prod = new product_inventory();
                        prod.productName = httprequest.Form["productName"];
                        prod.productCode = httprequest.Form["productCode"];
                        prod.product_quantity = Convert.ToInt32(httprequest.Form["productQuantity"]);
                        prod.product_price = Convert.ToDecimal(httprequest.Form["productPrice"]);
                        prod.product_supplier = httprequest.Form["productSupplier"];
                        prod.productimgurl = httprequest.Form["productImageUrl"];
                        prod.product_total = Convert.ToInt32(httprequest.Form["productPrice"]) * Convert.ToInt32(httprequest.Form["productQuantity"]);
                        prod.product_creator = httprequest.Form["isadmin"];
                        prod.product_status = httprequest.Form["isstatus"];
                        prod.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        prod.product_category = httprequest.Form["productcategory"];
                        core.product_inventory.Add(prod);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success product inventory");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-inventory-deletion"), HttpPost]
        public HttpResponseMessage proddeletion(int prodid)
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    if(prodid <= 0)
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
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-inventory-modification"), HttpPost]
        public HttpResponseMessage modifyproduct()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = new local_dbbmEntities())
                {
                    if(string.IsNullOrEmpty(httprequest.Form["modifyproductname"]) || string.IsNullOrEmpty(httprequest.Form["modifyproductimageurl"])
                        || string.IsNullOrEmpty(httprequest.Form["modifyproductquantity"]) || string.IsNullOrEmpty(httprequest.Form["modifyproductprice"])
                        || string.IsNullOrEmpty(httprequest.Form["modifycategory"]) || string.IsNullOrEmpty(httprequest.Form["modifyPID"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        core.update_product_inventory
                            (Convert.ToInt32(httprequest.Form["modifyPID"]), httprequest.Form["modifyproductname"],
                            Convert.ToInt32(httprequest.Form["modifyproductquantity"]), Convert.ToDecimal(httprequest.Form["modifyproductprice"]),
                            Convert.ToInt32(httprequest.Form["modifyproductprice"]) * Convert.ToInt32(httprequest.Form["modifyproductquantity"]),
                            httprequest.Form["modifyproductsupplier"], httprequest.Form["modifyproductimageurl"], httprequest.Form["modifycategory"], 1);
                        return Request.CreateResponse
                            (HttpStatusCode.OK, "success modify");
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
