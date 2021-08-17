using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.ProductFinalization
{
    [RoutePrefix("api/product-finalization")]
    public class productfinalizationController : ApiController
    {
        private local_dbbmEntities1 core;
        [Route("product-add"), HttpPost]
        public HttpResponseMessage prodadd(string prodname, int prodquantity, string prodcategory, decimal prodprice, string prodcode)
        {
            try
            {
                using (core = new local_dbbmEntities1())
                {
                    var httprequest = HttpContext.Current.Request;
                    if (string.IsNullOrEmpty(prodname) || string.IsNullOrEmpty(Convert.ToString(prodquantity))
                        || string.IsNullOrEmpty(Convert.ToString(prodprice)) || string.IsNullOrEmpty(httprequest.Form["prodimg"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        product_finalization prodfinal = new product_finalization();
                        prodfinal.prodname = prodname;
                        prodfinal.prodquantity = Convert.ToInt32(prodquantity);
                        prodfinal.prodcategory = prodcategory;
                        prodfinal.prodprice = Convert.ToDecimal(prodprice);
                        prodfinal.prodtotal = Convert.ToDecimal(prodprice) * Convert.ToInt32(prodquantity);
                        prodfinal.prodstatus = "0";
                        prodfinal.productCode = prodcode;
                        prodfinal.prodimg = httprequest.Form["prodimg"];
                        prodfinal.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.product_finalization.Add(prodfinal);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
                        //add product finalize

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("list-of-categories"), HttpGet]
        public HttpResponseMessage getcategories()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.tbcategories.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("list-of-stocks"), HttpGet]
        public HttpResponseMessage allstocks()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.product_inventory.Where(x => x.product_quantity != 0 && x.product_status == "1").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("filter-list-ingredients"), HttpGet]
        public HttpResponseMessage filterforlist(string category)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.product_inventory.Where(x => x.product_category == category && x.product_status == "1").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("selected-product"), HttpPost]
        public HttpResponseMessage selectedrawmaterial(string pname, string pcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var httprequest = HttpContext.Current.Request;
                    if(core.selectedraws.Any(x => x.prodcode == pcode))
                    {
                        //exist
                        return Request.CreateResponse(HttpStatusCode.OK, "exist");
                    }
                    else
                    {
                        selectedraw raw = new selectedraw();
                        raw.prodcode = pcode;
                        raw.prodname = pname;
                        raw.prodimg = httprequest.Form["prodimg"];
                        raw.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.selectedraws.Add(raw);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-selected-raw"), HttpGet]
        public HttpResponseMessage getrawselected()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.selectedraws.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("deletion-selection"), HttpPost]
        public HttpResponseMessage getridselection(int id)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if(id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                    }
                    else
                    {
                        var obj = core.selectedraws.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-product-code-selected-raw"), HttpGet]
        public HttpResponseMessage getallpcode()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.selectedraws.Select(x => new
                    {
                        x.prodcode
                    }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-deduct-quantity-product"), HttpPost]
        public HttpResponseMessage productquantitydeduction(int quantity, string pcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    
                        core.deductquantityfinal(pcode, quantity, 1);
                    return Request.CreateResponse(HttpStatusCode.OK, "success");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("clear-selected-raws"), HttpPost]
        public HttpResponseMessage clearraws()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    core.clearselectedraw(1);
                    return Request.CreateResponse(HttpStatusCode.OK, "clear");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-all-finalize-product"), HttpGet]
        public HttpResponseMessage getallfinalize()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.product_finalization.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        class graphrep
        {
            public int counter { get; set; }
            public object bulk { get; set; }
        }
        graphrep reps = new graphrep();
        [Route("get-all-finalize-graph"), HttpGet]
        public HttpResponseMessage bindgraphfinalizeproduct(bool filter)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if (filter)
                    {
                        var obj = core.product_finalization.Where(x => x.prodstatus == "1").ToList();
                        int count = core.product_finalization.Where(x => x.prodstatus == "1").ToList().Count();
                        reps.counter = count;
                        reps.bulk = obj;
                        return Request.CreateResponse(HttpStatusCode.OK, reps);
                    }
                    else
                    {
                        var obj = core.product_finalization.Where(x => x.prodstatus == "0").ToList();
                        int count = core.product_finalization.Where(x => x.prodstatus == "0").ToList().Count();
                        reps.counter = count;
                        reps.bulk = obj;
                        return Request.CreateResponse(HttpStatusCode.OK, reps);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-finalization-activator"), HttpPost]
        public HttpResponseMessage activatorproduct(int id)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    core.product_finalization_activator(id, 1);
                    return Request.CreateResponse(HttpStatusCode.OK, "success activate");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-finalization-deactivator"), HttpPost]
        public HttpResponseMessage deactivateproduct(int id)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    core.product_finalization_activator(id, 2);
                    return Request.CreateResponse(HttpStatusCode.OK, "success deactivate");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-finalization-get-activated-products"), HttpGet]
        public HttpResponseMessage getactivated()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.product_finalization.Where(x => x.prodstatus == "1").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-finalization-get-deactivated-products"), HttpGet]
        public HttpResponseMessage getdeactivated()
        {
            try
            {
                using (core = new local_dbbmEntities1())
                {
                    var obj = core.product_finalization.Where(x => x.prodstatus == "0").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        class uchiha
        {
            public object getobj { get; set; }
            public string msg { get; set; }
        }
        uchiha sasuke = new uchiha();
        [Route("product-finalization-remove"), HttpPost]
        public HttpResponseMessage removeproduct(int id, string pcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if(id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                    }
                    else
                    {
                        var remover = core.product_finalization.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(remover).State = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();

                        var obj = (from a in core.product_inventory
                                   join b in core.product_finalization_raw on a.productCode equals b.productInventoryCode
                                   where b.productInventoryCode == b.productInventoryCode && b.productCreatedCode == pcode
                                   select new
                                   {
                                       a.productCode,
                                       a.productName,
                                       a.productimgurl
                                   }).Distinct().ToList();
                        sasuke.getobj = obj;
                        sasuke.msg = "success remove";
                        return Request.CreateResponse(HttpStatusCode.OK, sasuke);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("asceding-quantity-product-inventory"), HttpPost]
        public HttpResponseMessage ascendquantity(int quantity, string pcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if(core.product_inventory.Any(x => x.productCode == pcode))
                    {
                        core.ascend_quantity(pcode, quantity, 1);
                    }
                    else
                    {
                        core.ascend_quantity(pcode, quantity, 2);
                    }
                    
                    return Request.CreateResponse(HttpStatusCode.OK, "success ascend");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("product-finalization-raw-history"), HttpPost]
        public HttpResponseMessage product_raw_history(string createdpcode, string inventorycode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    product_finalization_raw raw = new product_finalization_raw();
                    raw.productCreatedCode = createdpcode;
                    raw.productInventoryCode = inventorycode;
                    raw.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.product_finalization_raw.Add(raw);
                    core.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "success");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        class getallcodes
        {
            public object codegetters { get; set; }
            public object allget { get; set; }
        }
        getallcodes codegetter = new getallcodes();
        [Route("identify-product-code"), HttpGet]
        public HttpResponseMessage identifycode(string pcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    codegetter.codegetters = core.product_finalization_raw.Where(x => x.productCreatedCode == pcode).Select(y => new { 
                    y.productInventoryCode
                    }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, codegetter);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-all-product-from-inventory-via-code"), HttpGet]
        public HttpResponseMessage getallproducts(string inventorycode)
        {
            try
            {
                using (core = new local_dbbmEntities1())
                {
                    var obj = (from a in core.product_inventory join b in core.product_finalization_raw on a.productCode equals b.productInventoryCode
                               where b.productInventoryCode == b.productInventoryCode && b.productCreatedCode == inventorycode select new { 
                               a.productCode, a.productName, a.productimgurl
                               }).Distinct().ToList();
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
