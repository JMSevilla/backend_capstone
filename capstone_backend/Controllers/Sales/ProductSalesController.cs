using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using capstone_backend.globalCON;
using System.Web;
using System.Data.SqlClient;

namespace capstone_backend.Controllers.Sales
{
    [RoutePrefix("api/product-sales")]
    public class ProductSalesController : ApiController
    {
        dbbmEntities core;
        //class Response
        //{
        //    public string message { get; set; }
        //}
        //Response resp = new Response();
        [Route("get-sales"), HttpGet]
        public IHttpActionResult getSales()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = core.paymentDetails.Where(x => x.paymentStatus == "3" || x.paymentStatus == "2" || x.paymentStatus == "1")
                        .Select(y => new
                        {
                            y.orderInfo,
                            y.paymentInfo,
                            y.paymentID,
                            y.createdAt,
                            y.paymentStatus
                        }).ToList().Distinct();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-customer-json/{paymentID}"), HttpGet]
        public IHttpActionResult getcustomerjson(int paymentID)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = core.paymentDetails.Where(x => x.paymentID == paymentID)
                        .Select(y => new
                        {
                            y.paymentInfo
                        }).ToList().Distinct();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
