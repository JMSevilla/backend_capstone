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
    [RoutePrefix("api/login-history-management")]
    public class loginHistoryController : ApiController
    {
        loghistoryClass logs = new loghistoryClass();
        class Response
        {
            public string message { get; set; }
        }
        //private local_dbbmEntities1 core;
        private dbbmEntities core;
        Response resp = new Response();
        [Route("add-login-history"), HttpPost]
        public HttpResponseMessage addhistorylogin()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                logs.email = httprequest.Form["email"];
                logs.message = httprequest.Form["message"];
                logs.loggedinstatus = Convert.ToChar(httprequest.Form["status"]);
                logs.logindate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                if (string.IsNullOrEmpty(logs.email))
                {
                    resp.message = "empty email";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    using(core = apiglobalcon.publico)
                    {
                        login_history login = new login_history();
                        login.email = logs.email;
                        login.message = logs.message;
                        login.loggedinstatus = Convert.ToString(logs.loggedinstatus);
                        login.logindate = logs.logindate;
                        core.login_history.Add(login);
                        core.SaveChanges();
                        resp.message = "success";
                        return Request.CreateResponse(HttpStatusCode.OK, resp);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("add-logout-history"), HttpPut]
        public HttpResponseMessage addlogouthistory()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var latestLogID = core.login_history.OrderByDescending(a => a.id).First();
                    var checkLogout = core.login_history.Where(x => x.id == latestLogID.id).FirstOrDefault();
                    if(checkLogout != null)
                    {
                        checkLogout.loggedoutstatus = "0";
                        checkLogout.logoutdate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success_logout");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("list-of-login-histories"), HttpGet]
        public HttpResponseMessage listofhistories()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var obj = core.login_history.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("remove-list-history-login"), HttpPost]
        public HttpResponseMessage removehistorylogin(int id)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if(id <= 0)
                    {
                        resp.message = "invalid id";
                        return Request.CreateResponse(HttpStatusCode.OK, resp);
                    }
                    else
                    {
                        var remover = core.login_history.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(remover).State = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        resp.message = "success";
                        return Request.CreateResponse(HttpStatusCode.OK, resp);
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
