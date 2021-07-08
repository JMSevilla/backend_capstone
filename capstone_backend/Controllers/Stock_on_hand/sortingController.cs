﻿using System;
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
        private local_dbbmEntities core;
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
                using(core = new local_dbbmEntities())
                {
                    if(valbool == true)
                    {
                        DateTime curdate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                        var obj1 = core.expirations.Any(x => x.expirydate <= curdate);
                        if (obj1)
                        {
                            //exist expiration
                            var obj2 = core.expirations.Where(x => x.expirydate <= curdate).FirstOrDefault().pcode;
                            var obj = core.stock_on_hand.Where(x => x.stockNumber == obj2).ToList();
                            datamanage.bulk = obj;
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
