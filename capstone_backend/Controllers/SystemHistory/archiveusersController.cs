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
    [RoutePrefix("api/archive-users-management")]
    public class archiveusersController : ApiController
    {
        archiveUsersClass archive = new archiveUsersClass();
<<<<<<< HEAD
=======
        //private local_dbbmEntities1 core;
>>>>>>> 9721cfa66296c4d6926767be1ac2f5f3bb89c400
        private local_dbbmEntities1 core;
        class Response
        {
            public string message { get; set; }
        }
        Response resp = new Response();
        [Route("add-archive"), HttpPost]
        public HttpResponseMessage addarchive(int id)
        {
            try
            {
                var http = HttpContext.Current.Request;
                archive.clientID = Convert.ToInt32(http.Form["clientid"]);
                archive.archiveID = http.Form["archiveid"];
                archive.firstname = http.Form["firstname"];
                archive.lastname = http.Form["lastname"];
                archive.usertype = Convert.ToChar(http.Form["type"]);
                archive.archiveusermessage = http.Form["message"];
                archive.archivecreated = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                if(string.IsNullOrEmpty(archive.firstname)
                    || string.IsNullOrEmpty(archive.lastname))
                {
                    resp.message = "empty fields";
                    return Request.CreateResponse(HttpStatusCode
                        .OK, resp);
                }
                else
                {
                    using(core = new local_dbbmEntities1())
                    {
                        archive_users arr = new archive_users();
                        arr.archiveID = archive.archiveID;
                        arr.firstname = archive.firstname;
                        arr.lastname = archive.lastname;
                        arr.usertype = Convert.ToString(archive.usertype);
                        arr.archiveusermessage = archive.archiveusermessage;
                        arr.archiveCreated = archive.archivecreated;
                        arr.client_id = archive.clientID;
                        core.archive_users.Add(arr);
                        core.SaveChanges();
                        core.user_status_updater(id, 3);
                        resp.message = "success";
                        return Request.CreateResponse(HttpStatusCode.OK,
                            resp);
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("get-all-archives-users"), HttpGet]
        public HttpResponseMessage getallarchives()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.archive_users.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.OK,
                    ex.Message);
            }
        }
        [Route("recover-archive-users"), HttpPost]
        public HttpResponseMessage recoveruser(int id, int clientid)
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
                    using(core = new local_dbbmEntities1())
                    {
                        core.user_status_updater(clientid, 4);
                        var remover = core.archive_users.Where
                            (x => x.id == id).FirstOrDefault();
                        core.Entry(remover).State = System.Data.Entity.EntityState.Deleted;
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
    }
}
