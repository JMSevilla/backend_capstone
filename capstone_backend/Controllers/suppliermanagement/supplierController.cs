using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.suppliermanagement
{
    [RoutePrefix("api/supplier-management")]
    public class suppliermanagementController : ApiController
    {
        private local_dbbmEntities core;
        [Route("adding-supplier"), HttpPost]
        public HttpResponseMessage addsupplier()
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = new local_dbbmEntities())
                {
                    if(string.IsNullOrEmpty(httprequest.Form["supplierIC"]) || string.IsNullOrEmpty(httprequest.Form["supplierfirstname"])
                        || string.IsNullOrEmpty(httprequest.Form["supplierlastname"]) || string.IsNullOrEmpty(httprequest.Form["supplierprimary"])
                        || string.IsNullOrEmpty(httprequest.Form["suppliernumber"]) || string.IsNullOrEmpty(httprequest.Form["supplieremail"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        supplier supp = new supplier();
                        supp.supplierIdentificationCode = httprequest.Form["supplierIC"];
                        supp.supplierfirstname = httprequest.Form["supplierfirstname"];
                        supp.supplierlastname = httprequest.Form["supplierlastname"];
                        supp.supplierprimaryaddress = httprequest.Form["supplierprimary"];
                        supp.suppliersecondaryaddress = httprequest.Form["suppliersecondary"];
                        supp.suppliernumber = httprequest.Form["suppliernumber"];
                        supp.supplieremail = httprequest.Form["supplieremail"];
                        supp.supplierimgurl = httprequest.Form["supplierimgurl"];
                        supp.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                        supp.isstatus = "0";
                        core.suppliers.Add(supp);
                        core.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "success supplier");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("fetch-added-suppliers"), HttpGet]
        public HttpResponseMessage getAllSupplier()
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var obj = core.suppliers.Where(x => x.isstatus == "0").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("remove-supplier"), HttpPost]
        public HttpResponseMessage movetoarchive(int id)
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    if(id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                    }
                    else
                    {
                        core.stored_supplier(id, null, null, null, null, null, null, null, null, 1);
                        return Request.CreateResponse(HttpStatusCode.OK, "success archive");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("find-duplicate-supplier"), HttpGet]
        public HttpResponseMessage findduplicate()
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                   var obj = core.supplier_find_duplicate.Where(x => x.isstatus == "0").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("modification-supplier"), HttpPost]
        public HttpResponseMessage modifysupp()
        {
            using(core = new local_dbbmEntities())
            {
                var data = HttpContext.Current.Request;
                if(string.IsNullOrEmpty(data.Form["fname"]) || string.IsNullOrEmpty(data.Form["lname"])
                    || string.IsNullOrEmpty(data.Form["email"]) || string.IsNullOrEmpty(data.Form["number"]))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "empty");
                }
                else
                {
                    core.stored_supplier(Convert.ToInt32(data.Form["id"]), data.Form["fname"], data.Form["lname"],
                        null, null, data.Form["number"], data.Form["email"], data.Form["simg"], null, 2);
                    return Request.CreateResponse(HttpStatusCode.OK, "success modify");
                }
            }
        }
    }
}
