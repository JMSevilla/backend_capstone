using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.excelTemplate
{
    [RoutePrefix("api/save-excel")]
    public class excelController : ApiController
    {
<<<<<<< HEAD
=======
        //private local_dbbmEntities core;
>>>>>>> 9721cfa66296c4d6926767be1ac2f5f3bb89c400
        private local_dbbmEntities1 core;
        [Route("data-save"), HttpPost]
        public HttpResponseMessage saveexcel()
        {
            var httpreq = HttpContext.Current.Request;
            using (core = new local_dbbmEntities1())
            {
                excelStorage data = new excelStorage();
                data.tname = httpreq.Form["templatename"];
                data.turl = httpreq.Unvalidated["templateurl"];
                data.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                core.excelStorages.Add(data);
                core.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "success save");
            }
        }
    }
}
