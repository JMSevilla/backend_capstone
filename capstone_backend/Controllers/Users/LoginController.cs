using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.globalCON;
using capstone_backend.Models;
namespace capstone_backend.Controllers.Users
{
    [RoutePrefix("api/csrf-login")]
    public class LoginController : ApiController
    {
        //private local_dbbmEntities1 core;attemptUpdater
        private dbbmEntities core;
        //private dbbmEntities core1;
        APISecurity secure = new APISecurity();
        class Response
        {
            public object bundle { get; set; }
            public string response_message { get; set; }
        }
        Response resp = new Response();
        [Route("google-credentials-checker"), HttpPost]
        public HttpResponseMessage checkgoogle(string email)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var checkemail = core.user_google_allow.Any(x => x.g_email == email);
                    var getusertype = core.users.Where(x => x.email == email).FirstOrDefault();
                    if (checkemail)
                    {
                        if (getusertype.istype == "1")
                        {
                            var obj = core.users.Where(x => x.email == email);
                            resp.bundle = obj.Select(y => new {
                                y.firstname,
                                y.lastname,
                                y.email,
                                y.id,
                                y.istype
                            }).ToList();
                            resp.response_message = "proceed login admin";
                            return Request.CreateResponse(HttpStatusCode.OK, resp);
                        }
                        else
                        {
                            if (getusertype.isverified == "1")
                            {
                                if (getusertype.isstatus == "1")
                                {
                                    var obj = core.users.Where(x => x.email == email);
                                    resp.bundle = obj.Select(y => new {
                                        y.firstname,
                                        y.lastname,
                                        y.email,
                                        y.id,
                                        y.istype
                                    }).ToList();
                                    resp.response_message = "proceed login customer";
                                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "disable");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "not verified");
                            }
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "email not exists");
                    }
                }
                //if(core != null)
                // {

                // }
                // else
                // {
                //     using (core1 = new dbbmEntities())
                //     {
                //         var checkemail = core1.user_google_allow.Any(x => x.g_email == email);
                //         var getusertype = core1.users.Where(x => x.email == email).FirstOrDefault();
                //         if (checkemail)
                //         {
                //             if (getusertype.istype == "1")
                //             {
                //                 var obj = core1.users.Where(x => x.email == email);
                //                 resp.bundle = obj.Select(y => new {
                //                     y.firstname,
                //                     y.lastname,
                //                     y.email,
                //                     y.id,
                //                     y.istype
                //                 }).ToList();
                //                 resp.response_message = "proceed login admin";
                //                 return Request.CreateResponse(HttpStatusCode.OK, resp);
                //             }
                //             else
                //             {
                //                 if (getusertype.isverified == "1")
                //                 {
                //                     if (getusertype.isstatus == "1")
                //                     {
                //                         var obj = core1.users.Where(x => x.email == email);
                //                         resp.bundle = obj.Select(y => new {
                //                             y.firstname,
                //                             y.lastname,
                //                             y.email,
                //                             y.id,
                //                             y.istype
                //                         }).ToList();
                //                         resp.response_message = "proceed login customer";
                //                         return Request.CreateResponse(HttpStatusCode.OK, resp);
                //                     }
                //                     else
                //                     {
                //                         return Request.CreateResponse(HttpStatusCode.OK, "disable");
                //                     }
                //                 }
                //                 else
                //                 {
                //                     return Request.CreateResponse(HttpStatusCode.OK, "not verified");
                //                 }
                //             }
                //         }
                //         else
                //         {
                //             return Request.CreateResponse(HttpStatusCode.OK, "email not exists");
                //         }
                //     }
                // }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("add-update-session"), HttpPost]
        public HttpResponseMessage addsession(string email, int sessionid)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var check = core.sessionScans.Any(x => x.email == email);

                    if (check)
                    {
                        //update isused equal to 1
                        core.update_session_stats(email, "update_session");
                        return Request.CreateResponse(HttpStatusCode.OK, "update session");
                    }
                    else
                    {
                        sessionScan ses = new sessionScan();
                        ses.email = email;
                        ses.sessionID = sessionid;
                        ses.isused = "1";
                        ses.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.sessionScans.Add(ses);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "session added");
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("scan-oauth-session"), HttpPost]
        public HttpResponseMessage checksession(string email)
        {
            try
            {

                using (core = apiglobalcon.publico)
                {
                    if (core != null)
                    {
                        var session = core.users.Any(x => x.email == email && x.istoken != "");
                        if (session)
                        {
                            var userProfile = core.users.Where(y => y.email == email);
                            if (userProfile.FirstOrDefault().istype == "1")
                            {
                                if (userProfile.FirstOrDefault().isstatus == "1")
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "scan admin");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "account disabled");
                                }
                            }
                            else if (userProfile.FirstOrDefault().istype == "0")
                            {
                                if (userProfile.FirstOrDefault().isstatus == "1")
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "scan customer");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "account disabled");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "not found");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "homepage");
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
               
            }
            catch (Exception)
            {

                throw;

            }
        }
        [Route("account-logout"), HttpPost]
        public HttpResponseMessage destroySession(string email)
        {
            try
            {
               using(core = apiglobalcon.publico)
                {
                    if(core != null)
                    {
                        core.istokenupdater("", email, 1);
                        return Request.CreateResponse(HttpStatusCode.OK, "logout");
                    }
                    else
                    {
                        using (core = apiglobalcon.publico)
                        {
                            core.istokenupdater("", email, 1);
                            return Request.CreateResponse(HttpStatusCode.OK, "logout");
                        }
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("cashier-account-logout"), HttpPost]
        public HttpResponseMessage cashierdestroySession(string email)
        {
            try
            {

                using(core = apiglobalcon.publico)
                {
                    if(core != null)
                    {
                        core.istokenupdater("", email, 1);
                        return Request.CreateResponse(HttpStatusCode.OK, "logout");
                    }
                    else
                    {
                        using (core = apiglobalcon.publico)
                        {
                            core.istokenupdater("", email, 1);
                            return Request.CreateResponse(HttpStatusCode.OK, "logout");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        class LoginResponse
        {
            public string message { get; set; }
            public object databulk { get; set; }
            public string email { get; set; }
        }
        LoginResponse res = new LoginResponse();
        [Route("standard-login"), HttpPost]
        public HttpResponseMessage csrflogin()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using (core = apiglobalcon.publico)
                {
                    res.email = httprequest.Form["email"];
                    string pwd = secure.Encrypt(httprequest.Form["password"]);
                    string encrypted = string.Empty;
                    string istype;
                    string isstatus;
                    string isattempt;
                    var c1 = core.users.Any(x => x.email == res.email && x.isarchive != "1");
                    var c2 = core.users.Where(x => x.email == res.email).FirstOrDefault();
                    if (string.IsNullOrEmpty(res.email) || string.IsNullOrEmpty(pwd))
                    {
                        res.message = "empty";
                        return Request.CreateResponse(HttpStatusCode.OK, res);
                    }
                    else
                    {
                        if (c1)
                        {
                            encrypted = c2 == null ? "" : c2.password;
                            istype = c2.istype;
                            isstatus = c2.isstatus;
                            isattempt = c2.isattemptStatus;
                            string decryptoriginal = secure.Decrypt(pwd);
                            string decryptrequest = secure.Decrypt(encrypted);
                            if (isattempt == "1")
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "attempt failed");
                            }
                            else
                            {
                                if (decryptrequest == decryptoriginal)
                                {
                                    if (isstatus == "1")
                                    {
                                        if (istype == "1")
                                        {
                                            //admin

                                            var fetchy = core.users.Where(x => x.email == res.email).Select(t => new
                                            {
                                                t.id,
                                                t.firstname,
                                                t.lastname,
                                                t.istype,
                                                t.email
                                            }).ToList();
                                            res.databulk = fetchy.FirstOrDefault();
                                            res.message = "SUCCESS";
                                            IServiceToken(res.email, Guid.NewGuid().ToString());
                                            return Request.CreateResponse(HttpStatusCode.OK, res);
                                        }
                                        else
                                        {
                                            //cashier

                                            var cashier = core.users.Where(x => x.email == res.email).Select(t => new
                                            {
                                                t.id,
                                                t.firstname,
                                                t.lastname,
                                                t.istype,
                                                t.email
                                            }).ToList();
                                            res.databulk = cashier.FirstOrDefault();
                                            res.message = "SUCCESS CASHIER";
                                            IServiceToken(res.email, Guid.NewGuid().ToString());
                                            return Request.CreateResponse(HttpStatusCode.OK, res);
                                        }
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, "disabled");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "invalid");
                                }
                            }
                            
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "not found");
                        }
                    }
                }
                

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void IServiceToken(string email, string token)
        {
            using (apiglobalcon.publico)
            {
                apiglobalcon.publico.istokenupdater(token, email, 1);
            }
        }
        [Route("attemptUpdater"), HttpPut]
        public IHttpActionResult isAttemptUpdater(string email, int receivedCount)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {

                    var StringCounter = core.users.Where(x => x.email == email).FirstOrDefault();
                    int counter = Convert.ToInt32(StringCounter.isattemptCounter);
                    if (counter != 5)
                    {
                        //var handler = StringCounter.isattemptCounter = StringCounter.isattemptCounter + 1;
                        var res = core.users.Where(x => x.email == email).FirstOrDefault();
                        if(res != null)
                        {
                            res.isattemptCounter = receivedCount;
                            core.SaveChanges();

                        }
                        return Ok("isattemptcount update");
                    }
                    else
                    {
                        var updater = core.users.Where(x => x.email == email).FirstOrDefault();
                        if(updater != null)
                        {
                            updater.isattemptStatus = "3";
                            core.SaveChanges();
                        }
                        return Ok("success attempt update");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("lock-account"), HttpPut]
        public IHttpActionResult accountLock(string email)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var updater = core.users.Where(x => x.email == email).FirstOrDefault();
                    if (updater != null)
                    {
                        updater.isattemptStatus = "3";
                        core.SaveChanges();
                    }
                    return Ok("success attempt update");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-attempts"), HttpGet]
        public IHttpActionResult getattempts(string useremail)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    if(useremail == null)
                    {
                        return Ok("empty attempts");
                    }
                    else
                    {
                        var obj = core.users.Where(x => x.email == useremail).FirstOrDefault();
                        return Ok(obj.isattemptCounter);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-attempts-status"), HttpPut]
        public IHttpActionResult updateAttempts(string email)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = core.users.Where(x => x.email == email).FirstOrDefault();
                    if(obj != null)
                    {
                        obj.isattemptStatus = "1";
                        core.SaveChanges();
                        
                    }
                    return Ok("attempt status update");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("reset-attempts"), HttpPut]
        public IHttpActionResult resetAttempts(string email)
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    var obj = core.users.Where(x => x.email == email).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.isattemptStatus = "0";
                        obj.isattemptCounter = 0;
                        core.SaveChanges();

                    }
                    return Ok("reset attempt update");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
