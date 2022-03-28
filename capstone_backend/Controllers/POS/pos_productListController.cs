using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
namespace capstone_backend.Controllers.POS
{
    [RoutePrefix("api/pos")]
    public class pos_productListController : ApiController
    {
        [Route("products/get-all"), HttpGet]
        public IHttpActionResult getallprods()
        {
            try
            {
               
                var obj = apiglobalcon.publico.product_finalization.ToList().OrderByDescending(x => x.id);
                return Ok(obj);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("products/{barcode}/barcode"), HttpGet]
        public async Task<IHttpActionResult> getprodviaBarcode(string barcode)
        {
            try
            {
                using (apiglobalcon.publico)
                {
                    IHttpActionResult res = null;
                    var searcher = apiglobalcon.publico.product_finalization
                        .Where(x => x.productCode == barcode).ToList();
                    res = Ok(searcher);
                    return await Task.FromResult(res);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
