using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.Stock_on_hand
{
    [RoutePrefix("api/sort-stocks")]
    public class sortingController : ApiController
    {
        //private local_dbbmEntities1 core;
        private local_dbbmEntities1 core;
        class dataManagement
        {
            public object bulk { get; set; }
            public string msg { get; set; }
        }
        dataManagement datamanage = new dataManagement();
        [Route("sort-of-expired"), HttpGet]
        public HttpResponseMessage getexpiredfromstocks(bool valbool)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if(valbool == true)
                    {
                        DateTime curdate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                        var obj1 = core.stock_on_hand.Any(x => x.expirationprod <= curdate);
                        if (obj1)
                        {
                            //exist expiration
                            var obj2 = core.stock_on_hand.Where(x => x.expirationprod <= curdate).ToList();
                            datamanage.bulk = obj2;
                            datamanage.msg = "1";
                            return Request.CreateResponse(HttpStatusCode.OK, datamanage);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "not exist expiry");
                        }
                    }
                    else
                    {
                        var obj = core.stock_on_hand.ToList();
                        datamanage.bulk = obj;
                        datamanage.msg = "0";
                        return Request.CreateResponse(HttpStatusCode.OK, datamanage);
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
