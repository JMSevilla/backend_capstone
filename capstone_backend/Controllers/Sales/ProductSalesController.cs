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

                    DateTime today = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    var obj = core.product_sales.Where(x => x.createdAt == today).ToList().OrderByDescending(x => x.salesID);
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("filter-sales"), HttpGet]
        public IHttpActionResult filterSales(DateTime datefrom , DateTime dateto)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = (from filterdata in core.product_sales
                               where (filterdata.createdAt >= datefrom && filterdata.createdAt <= dateto)
                               orderby filterdata.salesID descending
                               select new
                               {
                                   filterdata.salesID,
                                   filterdata.salesInfo,
                                   filterdata.createdAt
                               }).ToList();

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
        //[Route("compute-sales-today"), HttpGet]
        //public IHttpActionResult salesTodayComputation()
        //{
        //    DateTime today = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
        //    string updateStatus = "select sum()";
        //    core.Database.ExecuteSqlCommand(updateStatus, new SqlParameter("@discount", "1"), new SqlParameter("@id", id), new SqlParameter("@orderprice", newAmount),
        //        new SqlParameter("@ordertotal", newAmount));
        //    return Ok("success discount");
        //}
    }
}
