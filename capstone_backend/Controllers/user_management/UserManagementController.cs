using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.globalCON;
using capstone_backend.Models;
namespace capstone_backend.Controllers.user_management
{
    [RoutePrefix("api/user-management")]
    public class UserManagementController : ApiController
    {
        //private local_dbbmEntities1 core;
        private dbbmEntities core;
        class Response
        {
            public string message { get; set; }
            public object bulk { get; set; }
        }
        Response resp = new Response();
        [Route("getall-users"), HttpGet]
        public HttpResponseMessage getAll()
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var objs = core.users.Where(x => x.isarchive == "0")
                       .Select(x => new
                       {
                           x.firstname,
                           x.lastname,
                           x.id,
                           x.email,
                           x.istype,
                           x.isstatus,
                           x.isverified,
                           x.createdAt,
                           x.isarchive
                       }).ToList();
                    resp.bulk = objs;
                    resp.message = "disregard current user";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("remove-permanent-user"), HttpPost]
        public HttpResponseMessage removepermanent(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                }
                else
                {
                    using(core = apiglobalcon.publico)
                    {
                        var remover = core.users.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(remover).State = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success delete");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-user-status"), HttpPost]
        public HttpResponseMessage userstatusupdated(int id, string indicator)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if(id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "invalid id");
                    }
                    else
                    {
                        if(indicator == "1")
                        {
                            core.user_status_updater(id, 1);
                            return Request.CreateResponse(HttpStatusCode.OK, "success deactivate");
                        }
                        else
                        {
                            core.user_status_updater(id, 2);
                            return Request.CreateResponse(HttpStatusCode.OK, "success activate");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("user-filtering"), HttpGet]
        public HttpResponseMessage filteruser(string value)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "empty");
                    }
                    else
                    {
                        if(value == "1") //fetch all admin
                        {
                            var obj_admin = core.users.Where(y => y.istype == "1")
                                .Select(x => new
                            {
                                x.firstname,
                                x.lastname,
                                x.id,
                                x.email,
                                x.istype,
                                x.isstatus,
                                x.isverified,
                                x.createdAt,
                                x.isarchive
                            }).Distinct().ToList();
                            return Request.CreateResponse(HttpStatusCode.Accepted, obj_admin);
                        }
                        else 
                        {
                            //cashiers
                            var obj_cashier = core.users.Where(y => y.istype == "0")
                                .Select(x => new
                                {
                                    x.firstname,
                                    x.lastname,
                                    x.id,
                                    x.email,
                                    x.istype,
                                    x.isstatus,
                                    x.isverified,
                                    x.createdAt,
                                    x.isarchive
                                }).Distinct().ToList();
                            return Request.CreateResponse(HttpStatusCode.Accepted, obj_cashier);
                        }
                        
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("archive-user"), HttpPost]
        public IHttpActionResult archive(int id, int indicator)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if(id <= 0)
                    {
                        return BadRequest("Invalid ID");
                    }
                    else
                    {
                        if(indicator == 3)
                        {
                            core.user_status_updater(id, indicator);
                            return Ok("Success archive");
                        }
                        else
                        {
                            core.user_status_updater(id, indicator);
                            return Ok("Success retrieve");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("archive-list"), HttpGet]
        public HttpResponseMessage getarchive(int indicator)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    if (indicator <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid indicator");
                    }
                    else
                    {
                        if(indicator == 1)
                        {
                            var objadmin = core.users.Where(x =>  x.istype == "1")
                                .Select(x => new
                                {
                                    x.firstname,
                                    x.lastname,
                                    x.id,
                                    x.email,
                                    x.istype,
                                    x.isstatus,
                                    x.isverified,
                                    x.createdAt, x.isarchive
                                }).ToList().Distinct();
                            return Request.CreateResponse(HttpStatusCode.OK, objadmin);
                        }
                        else if (indicator == 2)
                        {
                            var objcashier = core.users.Where(x => x.istype == "2")
                                .Select(x => new
                                {
                                    x.firstname,
                                    x.lastname,
                                    x.id,
                                    x.email,
                                    x.istype,
                                    x.isstatus,
                                    x.isverified,
                                    x.createdAt,
                                    x.isarchive
                                }).ToList().Distinct();
                            return Request.CreateResponse(HttpStatusCode.OK, objcashier);
                        }
                        else 
                        {
                            var objcustomer = core.users.Where(x => x.istype == "0")
                                .Select(x => new
                                {
                                    x.firstname,
                                    x.lastname,
                                    x.id,
                                    x.email,
                                    x.istype,
                                    x.isstatus,
                                    x.isverified,
                                    x.createdAt,
                                    x.isarchive
                                }).ToList().Distinct();
                            return Request.CreateResponse(HttpStatusCode.OK, objcustomer);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        APISecurity security = new APISecurity();
        [Route("adding-admin"), HttpPost]
        public HttpResponseMessage addingadmin()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = apiglobalcon.publico)
                {
                    var emailHandler = httprequest.Form["email"];
                    var checkUser = core.users.Where(x => x.email == emailHandler).FirstOrDefault();
                    if(checkUser != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "email exist admin");
                    }
                    else
                    {
                        user tbuser = new user();
                        tbuser.firstname = httprequest.Form["firstname"];
                        tbuser.lastname = httprequest.Form["lastname"];
                        tbuser.email = httprequest.Form["email"];
                        tbuser.password = security.Encrypt(httprequest.Form["password"]);
                        tbuser.istype = httprequest.Form["istype"];
                        tbuser.isverified = httprequest.Form["isverified"];
                        tbuser.isstatus = httprequest.Form["isstatus"];
                        tbuser.is_google_verified = httprequest.Form["is_google_verified"];
                        tbuser.imageurl = httprequest.Form["imageurl"];
                        tbuser.isarchive = "0";
                        tbuser.istoken = "";
                        tbuser.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.users.Add(tbuser);
                        core.SaveChanges();
                        //google table

                        user_google_allow google = new user_google_allow();
                        google.g_email = httprequest.Form["email"];
                        core.user_google_allow.Add(google);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        APISecurity cashiersecurity = new APISecurity();
        [Route("adding-cashier"), HttpPost]
        public HttpResponseMessage addingcashier()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using (core = apiglobalcon.publico)
                {
                    var emailHandler = httprequest.Form["email"];
                    var checkUser = core.users.Where(x => x.email == emailHandler).FirstOrDefault();
                    if(checkUser != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "email exist cashier");
                    }
                    else
                    {
                        user tbuser = new user();
                        tbuser.firstname = httprequest.Form["firstname"];
                        tbuser.lastname = httprequest.Form["lastname"];
                        tbuser.email = httprequest.Form["email"];
                        tbuser.password = cashiersecurity.Encrypt(httprequest.Form["password"]);
                        tbuser.istype = httprequest.Form["istype"];
                        tbuser.isverified = httprequest.Form["isverified"];
                        tbuser.isstatus = httprequest.Form["isstatus"];
                        tbuser.is_google_verified = httprequest.Form["is_google_verified"];
                        tbuser.imageurl = httprequest.Form["imageurl"];
                        tbuser.isarchive = "0";
                        tbuser.istoken = "";
                        tbuser.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        core.users.Add(tbuser);
                        core.SaveChanges();
                        //google table

                        user_google_allow google = new user_google_allow();
                        google.g_email = httprequest.Form["email"];
                        core.user_google_allow.Add(google);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
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
