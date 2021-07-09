using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.Users
{
    [RoutePrefix("api/csrf-login")]
    public class LoginController : ApiController
    {
        private local_dbbmEntities core;
        private dbbmEntities core1;
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
                using (core = new local_dbbmEntities())
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
                using (core = new local_dbbmEntities())
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
                //if (core != null)
                //{
                    
                //}
                //else
                //{
                //    //using (core1 = new dbbmEntities())
                //    //{
                //    //    var check = core1.sessionScans.Any(x => x.email == email);
                //    //    if (check)
                //    //    {
                //    //        //update isused equal to 1
                //    //        core1.update_session_stats(email, "update_session");
                //    //        return Request.CreateResponse(HttpStatusCode.OK, "update session");
                //    //    }
                //    //    else
                //    //    {
                //    //        sessionScan ses = new sessionScan();
                //    //        ses.email = email;
                //    //        ses.sessionID = sessionid;
                //    //        ses.isused = "1";
                //    //        ses.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                //    //        core.sessionScans.Add(ses);
                //    //        core.SaveChanges();
                //    //        return Request.CreateResponse(HttpStatusCode.OK, "session added");
                //    //    }
                //    //}
                //}
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

                using (core = new local_dbbmEntities())
                {
                    if (core != null)
                    {
                        var session = core.sessionScans.Any(x => x.email == email && x.isused == "1");
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
                    else
                    {
                        using (core1 = new dbbmEntities())
                        {
                            var session = core1.sessionScans.Any(x => x.email == email && x.isused == "1");
                            if (session)
                            {
                                var userProfile = core1.users.Where(y => y.email == email);
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
                    }
                   
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
               using(core = new local_dbbmEntities())
                {
                    if(core != null)
                    {
                        core.update_session_stats(email, "logout_session");
                        return Request.CreateResponse(HttpStatusCode.OK, "logout");
                    }
                    else
                    {
                        using (core1 = new dbbmEntities())
                        {
                            core1.update_session_stats(email, "logout_session");
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
                using (core = new local_dbbmEntities())
                {
                    res.email = httprequest.Form["email"];
                    string pwd = secure.Encrypt(httprequest.Form["password"]);
                    string encrypted = string.Empty;
                    string istype;
                    string isstatus;
                    var c1 = core.users.Any(x => x.email == res.email);
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
                            string decryptoriginal = secure.Decrypt(pwd);
                            string decryptrequest = secure.Decrypt(encrypted);
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
                                            t.istype
                                        }).ToList();
                                        res.databulk = fetchy.FirstOrDefault();
                                        res.message = "SUCCESS";
                                        return Request.CreateResponse(HttpStatusCode.OK, res);
                                    }
                                    else
                                    {
                                        //customer
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK);
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
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "not found");
                        }
                    }
                }
                //if(core != null)
                // {

                // }
                // else
                // {
                //     using (core1 = new dbbmEntities())
                //     {
                //         res.email = httprequest.Form["email"];
                //         string pwd = secure.Encrypt(httprequest.Form["password"]);
                //         string encrypted = string.Empty;
                //         string istype;
                //         string isstatus;
                //         var c1 = core1.users.Any(x => x.email == res.email);
                //         var c2 = core1.users.Where(x => x.email == res.email).FirstOrDefault();
                //         if (string.IsNullOrEmpty(res.email) || string.IsNullOrEmpty(pwd))
                //         {
                //             res.message = "empty";
                //             return Request.CreateResponse(HttpStatusCode.OK, res);
                //         }
                //         else
                //         {
                //             if (c1)
                //             {
                //                 encrypted = c2 == null ? "" : c2.password;
                //                 istype = c2.istype;
                //                 isstatus = c2.isstatus;
                //                 string decryptoriginal = secure.Decrypt(pwd);
                //                 string decryptrequest = secure.Decrypt(encrypted);
                //                 if (decryptrequest == decryptoriginal)
                //                 {
                //                     if (isstatus == "1")
                //                     {
                //                         if (istype == "1")
                //                         {
                //                             admin

                //                             var fetchy = core1.users.Where(x => x.email == res.email).Select(t => new
                //                             {
                //                                 t.id,
                //                                 t.firstname,
                //                                 t.lastname,
                //                                 t.istype
                //                             }).ToList();
                //                             res.databulk = fetchy.FirstOrDefault();
                //                             res.message = "SUCCESS";
                //                             return Request.CreateResponse(HttpStatusCode.OK, res);
                //                         }
                //                         else
                //                         {
                //                             customer
                //                         }
                //                         return Request.CreateResponse(HttpStatusCode.OK);
                //                     }
                //                     else
                //                     {
                //                         return Request.CreateResponse(HttpStatusCode.OK, "disabled");
                //                     }
                //                 }
                //                 else
                //                 {
                //                     return Request.CreateResponse(HttpStatusCode.OK, "invalid");
                //                 }
                //             }
                //             else
                //             {
                //                 return Request.CreateResponse(HttpStatusCode.OK, "not found");
                //             }
                //         }
                //     }
                //}

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
