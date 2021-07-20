using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.SystemHistory
{
    [RoutePrefix("api/activity-log-management")]
    public class ActivityLogController : ApiController
    {
        private local_dbbmEntities core;
        ActivityLogClass activity = new ActivityLogClass();
        class Response
        {
            public string message { get; set; }
        }
        Response resp = new Response();
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
                using(core = new local_dbbmEntities())
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
                using(core = new local_dbbmEntities())
                {
                    var obj = core.activity_log.ToList();
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
