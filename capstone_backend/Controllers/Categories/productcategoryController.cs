using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using capstone_backend.Models;
using capstone_backend.globalCON;
namespace capstone_backend.Controllers.Categories
{
    [RoutePrefix("api/product-category-management")]
    public class productcategoryController : ApiController
    {
        //private local_dbbmEntities core;
       
        //private local_dbbmEntities db = new local_dbbmEntities();
        [Route("add-category"), HttpPost]
        public HttpResponseMessage addcategory(string categoryname)
        {
            try
            {
                using(dbbmEntities core = apiglobalcon.publico)
                {   
                    if (string.IsNullOrWhiteSpace(categoryname))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                       if(core.tbcategories.Any(x => x.categoryname == categoryname))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "exist");
                        }
                        else
                        {
                            tbcategory categ = new tbcategory();
                            categ.categoryname = categoryname;
                            categ.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                            core.tbcategories.Add(categ);
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("add-category-final"), HttpPost]
        public HttpResponseMessage addcategfinal(string categoryname)
        {
            try
            {
                using (dbbmEntities core = apiglobalcon.publico)
                {
                    if (string.IsNullOrWhiteSpace(categoryname))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty");
                    }
                    else
                    {
                        if (core.tbcategoryfinals.Any(x => x.categoryname == categoryname))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "exist");
                        }
                        else
                        {
                            tbcategoryfinal categs = new tbcategoryfinal();
                            categs.categoryname = categoryname;
                            categs.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                            core.tbcategoryfinals.Add(categs);
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.OK, ex.Message);
            }
        }
        [Route("get-list-category-final"), HttpGet]
        public HttpResponseMessage listcategoryfinal()
        {
            try
            {
                using (dbbmEntities core = apiglobalcon.publico)
                {
                    var obj = core.tbcategoryfinals.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-category-name-final"), HttpPut]
        public HttpResponseMessage editcategoryfinal(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                }
                else
                {
                    using (dbbmEntities core = apiglobalcon.publico)
                    {
                        var obj = core.tbcategoryfinals.Where(x => x.id == id).FirstOrDefault();
                        if (obj != null)
                        {
                            var http = HttpContext.Current.Request;
                            obj.categoryname = http.Form["categ"];
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success update");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "invalid id not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("update-category-name"), HttpPut]
        public HttpResponseMessage editcategory(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                }
                else
                {
                    using(dbbmEntities core= apiglobalcon.publico)
                    {
                        var obj = core.tbcategories.Where(x => x.id == id).FirstOrDefault();
                        if (obj != null)
                        {
                            var http = HttpContext.Current.Request;
                            obj.categoryname = http.Form["categ"];
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success update");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "invalid id not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("get-list-category"), HttpGet]
        public HttpResponseMessage listcategory()
        {
            try
            {
                using(dbbmEntities core = apiglobalcon.publico)
                {
                    var obj = core.tbcategories.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
       [Route("remove-category"), HttpPost]
       public HttpResponseMessage categoryremove(int id)
        {
            try
            {
                using(dbbmEntities core = apiglobalcon.publico)
                {
                    if(id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                    }
                    else
                    {
                        var remove = core.tbcategories.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(remove).State = System.Data.Entity.EntityState.Deleted;
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
        [Route("remove-category-final"), HttpDelete]
        public HttpResponseMessage categoryremovefinal(int id)
        {   
            try
            {
                using (dbbmEntities core = apiglobalcon.publico)
                {
                    if (id <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid id");
                    }
                    else
                    {
                        var remove = core.tbcategoryfinals.Where(x => x.id == id).FirstOrDefault();
                        core.Entry(remove).State = System.Data.Entity.EntityState.Deleted;
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
    }
}
