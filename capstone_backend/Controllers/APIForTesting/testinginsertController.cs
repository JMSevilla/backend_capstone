using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.APIForTesting
{
    [RoutePrefix("api/testing-adding")]
    public class testinginsertController : ApiController
    {
        class dataStructure
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public DateTime createdat { get; set; }
            public string message { get; set; }
        }
        dataStructure data = new dataStructure();
        private local_dbbmEntities core;
        [Route("adding"), HttpPost]
        public HttpResponseMessage addService()
        {
            try
            {
                var http = HttpContext.Current.Request;
                data.firstname = http.Form["firstname"];
                data.lastname = http.Form["lastname"];
                data.createdat = Convert.ToDateTime
                    (System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                if(string.IsNullOrEmpty(data.firstname) 
                    || string.IsNullOrEmpty(data.lastname))
                {
                    data.message = "empty fields";
                    return Request.CreateResponse
                        (HttpStatusCode.OK, data);
                }
                else
                {
                    using(core = new local_dbbmEntities())
                    {
                        tbtesting test = new tbtesting();
                        test.firstname = data.firstname;
                        test.lastname = data.lastname;
                        test.createdAt = data.createdat;
                        core.tbtestings.Add(test);
                        core.SaveChanges();
                        data.message = "success";
                        return Request.CreateResponse
                            (HttpStatusCode.OK, data);
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
