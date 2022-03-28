using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
using capstone_backend.globalCON;

namespace capstone_backend.Controllers.SystemHistory
{
    [RoutePrefix("api/activity-log-management")]
    public class ActivityLogController : ApiController
    {
        //private local_dbbmEntities1 core;
        private dbbmEntities core;
        ActivityLogClass activity = new ActivityLogClass();
        class Response
        {
            public string message { get; set; }
        }
        Response resp = new Response();
        [Route("recover-product-archive"), HttpPut]
        public IHttpActionResult recoverProduct(int stockID)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var check = core.stock_on_hand.Where(x => x.stockID == stockID).FirstOrDefault();
                    if(check != null)
                    {
                        check.productstatus = "1";
                        core.SaveChanges();
                        
                    }
                    return Ok("success recover");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("remove-product-archive"), HttpDelete]
        public IHttpActionResult removeProductArchive(int stockID)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var check = core.stock_on_hand.Where(x => x.stockID == stockID).FirstOrDefault();
                    core.Entry(check).State = System.Data.Entity.EntityState.Deleted;
                    core.SaveChanges();
                    return Ok("success deleted");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("add-activity-log-user-management"), HttpPost]
        public HttpResponseMessage activitylog_user_management()
        {
            try
            {
                var http = HttpContext.Current.Request;
                activity.activitymessage = http.Form["message"];
                activity.activitystatus = http.Form["status"];
                activity.activitycode = http.Form["code"];
                activity.createdat = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                using(core = apiglobalcon.publico)
                {
                    activity_log act = new activity_log();
                    act.activitymessage = activity.activitymessage;
                    act.activtystatus = activity.activitystatus;
                    act.activityCode = activity.activitycode;
                    act.createdAt = activity.createdat;
                    core.activity_log.Add(act);
                    core.SaveChanges();
                    resp.message = "success";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-all-user-management-logs"), HttpGet]
        public HttpResponseMessage userlogs()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.activity_log.Where(x => x.activtystatus == "User Management").ToList().OrderByDescending(y => y.id);
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-all-product-activation-logs"), HttpGet]
        public HttpResponseMessage productactivationlogs()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.activity_log.Where
                        (x => x.activtystatus == "Product Inventory Activation")
                        .ToList();
                    return Request.CreateResponse
                        (HttpStatusCode.OK, obj);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse
                    (HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("remove-activity-log-user-management"), HttpPost]
        public HttpResponseMessage removeactivitylogusermanagement(int id)
        {
            try
            {
                if(id <= 0)
                {
                    resp.message = "invalid id";
                    return Request.CreateResponse
                        (HttpStatusCode.OK, resp);
                }
                else
                {
                    using(core = apiglobalcon.publico)
                    {
                        var remover =
                            core.activity_log.Where(x => x.id == id)
                            .FirstOrDefault();
                        core.Entry(remover).State
                            = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        resp.message = "success";
                        return Request.CreateResponse
                            (HttpStatusCode.OK, resp);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse
                    (HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("remove-product-activation-logs"), HttpPost]
        public HttpResponseMessage prodlogsremoving(int id)
        {
            try
            {
                if(id <= 0)
                {
                    resp.message = "invalid id";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    using(core = apiglobalcon.publico)
                    {
                        var remover =
                            core.activity_log.Where
                            (x => x.id == id).FirstOrDefault();
                        core.Entry(remover).State
                            = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        resp.message = "success";
                        return Request.CreateResponse
                            (HttpStatusCode.OK, resp);
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse
                    (HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("add-activity-log-activation-products"), HttpPost]
        public HttpResponseMessage activitylog_activation_products()
        {
            try
            {
                var http = HttpContext.Current.Request;
                activity.activitymessage = http.Form["message"];
                activity.activitystatus = http.Form["status"];
                activity.activitycode = http.Form["code"];
                activity.createdat = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                using (core = apiglobalcon.publico)
                {
                    activity_log act = new activity_log();
                    act.activitymessage = activity.activitymessage;
                    act.activtystatus = activity.activitystatus;
                    act.activityCode = activity.activitycode;
                    act.createdAt = activity.createdat;
                    core.activity_log.Add(act);
                    core.SaveChanges();
                    resp.message = "success";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse
                    (HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
