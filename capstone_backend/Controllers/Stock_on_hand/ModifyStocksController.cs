using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
using System.Web;

namespace capstone_backend.Controllers.Stock_on_hand
{
    [RoutePrefix("api/modificationstocks")]
    public class ModifyStocksController : ApiController
    {
        private dbbmEntities core;

        [Route("updatestocksproducts"), HttpPost]
        public IHttpActionResult modifyProduct()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var http = HttpContext.Current.Request;
                    if (http.Form["prodexp"] == null || http.Form["prodexp"] == "")
                    {
                        core.update_stocks_modification(Convert.ToInt32(http.Form["id"]), http.Form["prodname"], http.Form["prodcateg"], null, "modify_product");
                        return Ok("success update");
                    }
                    else
                    {
                        core.update_stocks_modification(Convert.ToInt32(http.Form["id"]), http.Form["prodname"], http.Form["prodcateg"], Convert.ToDateTime(http.Form["prodexp"]), "modify_product");
                        return Ok("success update");
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
