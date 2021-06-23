using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers
{
    [RoutePrefix("api/adding-data")]
    public class AddingController : ApiController
    {
        private dbtrainingEntities core;
        InsertClass _data = new InsertClass();
        [Route("inserting"), HttpPost]
        public HttpResponseMessage addingdata()
        {
            try
            {
                var request_client = HttpContext.Current.Request;
                _data.insert(request_client.Form["firstname"],
                    request_client.Form["lastname"],
                    request_client.Form["email"]);
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("getsomething"), HttpGet]
        public HttpResponseMessage getAll()
        {
            try
            {
                using(core = new dbtrainingEntities())
                {
                    var obj = core.tbinformations.Select(x => new
                    {
                        x.id,
                        x.firstname,
                        x.lastname,
                        x.email,
                        x.createdAt
                    }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("remove-data"), HttpPost]
        public HttpResponseMessage removeById(int id)
        {
            try
            {
                using(core = new dbtrainingEntities())
                {
                    if(id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid ID");
                    }
                    else
                    {
                        var deletion = core.tbinformations.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(deletion).State = System.Data.Entity.EntityState.Deleted;
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.Accepted, "Success");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-data"),HttpPost]
        public HttpResponseMessage updatesomething()
        {
            try
            {
                var _request = HttpContext.Current.Request;
                using(core = new dbtrainingEntities())
                {
                    core.updateinformation(
                            Convert.ToInt32(_request.Form["id"]), _request.Form["firstname"],
                            _request.Form["lastname"], _request.Form["email"], "update");
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
