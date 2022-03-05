using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.globalCON;
using capstone_backend.Models;
using System.Data.SqlClient;
namespace capstone_backend.Controllers.InventoryReports
{
    [RoutePrefix("api/inventory-reports")]
    public class InventoryReportsController : ApiController
    {
        private dbbmEntities core;
        [Route("check-on-openstore-mode"), HttpGet]
        public IHttpActionResult checkOpenStore()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    DateTime today = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    var checkInv = core.inventoryReports.Where(x => x.createdAt >= today).FirstOrDefault();
                    if(checkInv != null)
                    {
                        return Ok("exist today");
                    }
                    else
                    {
                        return Ok("not exist today");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public IHttpActionResult invreportEntry(string prodname, int beg, int refId)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    inventoryReport inv = new inventoryReport();
                    inv.productName = prodname;
                    inv.beg_qty = beg;
                    inv.available = "";
                    inv.end_qty = 0;
                    inv.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    inv.refId = refId;
                    inv.refstatus = "1";
                    core.inventoryReports.Add(inv);
                    core.SaveChanges();
                    return Ok("success entry");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-inventory-reports"), HttpGet]
        public IHttpActionResult getreports()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    DateTime today = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    var obj = core.inventoryReports.Where(x => x.createdAt == today).ToList();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("filter-inventory-reports"), HttpGet]
        public IHttpActionResult filterSales(DateTime datefrom, DateTime dateto)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = (from filterdata in core.inventoryReports
                               where (filterdata.createdAt >= datefrom && filterdata.createdAt <= dateto)
                               orderby filterdata.invID descending
                               select new
                               {
                                   filterdata.invID,
                                   filterdata.productName,
                                   filterdata.beg_qty,
                                   filterdata.available,
                                   filterdata.end_qty,
                                   filterdata.createdAt,
                                   filterdata.refId,
                                   filterdata.refstatus,
                               }).ToList();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-quantity-current-inventory"), HttpPut]
        public IHttpActionResult getandUpdate(string refId)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var qty = core.product_inventory.Where(x => x.productCode == refId).FirstOrDefault();
                    if(qty != null)
                    {
                        string updateEND = "update inventoryReports set end_qty=@qty, refstatus=@stat where refId=@refid";
                        core.Database.ExecuteSqlCommand(updateEND, new SqlParameter("@qty", qty.product_quantity), new SqlParameter("@stat", "2"), new SqlParameter("@refid", refId));
                        return Ok("success update end");
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-product-per-day"), HttpGet]
        public IHttpActionResult getprodperday(DateTime today)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = core.inventoryReports.Where(x => x.createdAt == today).ToList();
                    return Ok(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("send-product-to-beg"), HttpPost]
        public IHttpActionResult sendprodbeg(string prodname, int beg, int refid)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    inventoryReport inv = new inventoryReport();
                    inv.productName = prodname;
                    inv.beg_qty = beg;
                    inv.refId = refid;
                    inv.available = "";
                    inv.end_qty = 0;
                    inv.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    inv.refstatus = "1";
                    core.inventoryReports.Add(inv);
                    core.SaveChanges();
                    return Ok("success send");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("check-inventory-reports-exists"), HttpPut]
        public IHttpActionResult checkReports(string prodname, int beg, int refId)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    DateTime today = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    var checkObj = core.inventoryReports.Where(x => x.refstatus == "1" && x.refId == refId && x.createdAt == today).FirstOrDefault();
                    if (checkObj != null)
                    {
                        checkObj.beg_qty = checkObj.beg_qty + beg;
                        core.SaveChanges();
                        return Ok("success update");
                        
                    }
                    else
                    {
                        invreportEntry(prodname, beg, refId);
                        return Ok("success entry");
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
