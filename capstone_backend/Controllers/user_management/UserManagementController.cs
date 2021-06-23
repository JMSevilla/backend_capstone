using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.user_management
{
    [RoutePrefix("api/user-management")]
    public class UserManagementController : ApiController
    {
        private local_dbbmEntities core;
        class Response
        {
            public string message { get; set; }
        }
        [Route("getall-users"), HttpGet]
        public HttpResponseMessage getAll()
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var objs = core.users
                        .Select(x => new
                    {
                        x.firstname, x.lastname, x.id, x.email,
                        x.istype, x.isstatus, x.isverified, x.createdAt, x.isarchive
                    }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, objs);
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
                using(core = new local_dbbmEntities())
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
                using(core = new local_dbbmEntities())
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "empty");
                    }
                    else
                    {
                        if(value == "1") //fetch all admin
                        {
                            var obj_admin = core.users.Where(y => y.istype == value)
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
                        else if(value == "2")
                        {
                            //cashiers
                            var obj_cashier = core.users.Where(y => y.istype == value)
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
                        else
                        {
                            //customers
                            var obj_customers = core.users.Where(y => y.istype == value)
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
                            return Request.CreateResponse(HttpStatusCode.Accepted, obj_customers);
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
                using(core = new local_dbbmEntities())
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
                using(core = new local_dbbmEntities())
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
                using(core = new local_dbbmEntities())
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
                using (core = new local_dbbmEntities())
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
