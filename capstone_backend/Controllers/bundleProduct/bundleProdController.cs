using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
using System.Threading.Tasks;
using System.Web;
namespace capstone_backend.Controllers.bundleProduct
{
    [RoutePrefix("api/bundle")]
    public class bundleProdController : ApiController
    {
        local_dbbmEntities2 core;
        [Route("fetchAll-prodfinal"), HttpGet]
        public async Task<IHttpActionResult> getall()
        {
            try
            {
                IHttpActionResult result = null;
                using(core = apiglobalcon.publico)
                {
                    var obj = core.product_finalization.ToList().Distinct();
                    result = Ok(obj);
                    return await Task.FromResult(result);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("add-bundle"), HttpPost]
        public IHttpActionResult addbundle()
        {
            try
            {
                var HTTP = HttpContext.Current.Request;
                bundleProd bundle = new bundleProd();
                using (core = apiglobalcon.publico)
                {
                    bundle.bundleTitle = HTTP.Form["title"];
                    bundle.bundleIntegratedProdFinal = HTTP.Form["prodfinal"];
                    bundle.bundleIntegratedProdInvID = HTTP.Form["prodinvID"];
                    bundle.bundleQuantity = Convert.ToInt32(HTTP.Form["bundlequantity"]);
                    bundle.prodPrice = Convert.ToDecimal(HTTP.Form["prodprice"]);
                    bundle.prodImg = HTTP.Form["prodimg"];
                    bundle.bundleCode = Guid.NewGuid().ToString();
                    bundle.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM"));
                    bundle.isvalidate = "1";
                    bundle.isbundle = "1";
                    core.bundleProds.Add(bundle);
                    core.SaveChanges();
                    return Ok("success bundle");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-specific-prod-inv-id/{prodname}"), HttpGet]
        public IHttpActionResult getprodinv(string prodname)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var selected = core.product_finalization.Where
                        (x => x.prodname == prodname)
                        .Select(y => new
                        {
                            y.integratedRaws,
                            y.prodquantity
                        }).ToList();
                    return Ok(selected);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("fetchAll-bundle"), HttpGet]
        public async Task<IHttpActionResult> getallbundle()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    IHttpActionResult res = null;
                    var ob = core.bundleProds.ToList().Distinct();
                    res = Ok(ob);
                    return await Task.FromResult(res);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("validate-bundle/{bundlename}/{quantity}"), HttpGet]
        public IHttpActionResult validatebundle(string bundlename, int quantity)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = core.product_finalization.Where(x => x.prodname == bundlename).FirstOrDefault();
                    if(obj != null)
                    {
                        if(quantity > obj.prodquantity)
                        {
                            return Ok("invalid validation");
                        }
                        else
                        {
                            return Ok("validation passed");
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
        [Route("validate-passed/{bundlename}/{quantity}"), HttpPut]
        public IHttpActionResult passedval(string bundlename, int quantity)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.product_finalization.Where(x => x.prodname == bundlename).FirstOrDefault();
                    if(obj != null)
                    {
                        if (quantity > obj.prodquantity)
                        {
                            return Ok("invalid validation");
                        }
                        else
                        {
                            obj.prodquantity = obj.prodquantity - quantity;
                            core.SaveChanges();
                            return Ok("success decreased");
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
    }
}
