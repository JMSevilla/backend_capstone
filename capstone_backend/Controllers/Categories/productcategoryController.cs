using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using capstone_backend.Models;
namespace capstone_backend.Controllers.Categories
{
    [RoutePrefix("api/product-category-management")]
    public class productcategoryController : ApiController
    {
        private local_dbbmEntities1 core;
        [Route("add-category"), HttpPost]
        public HttpResponseMessage addcategory(string categoryname)
        {
            try
            {
                using(core = new local_dbbmEntities1())
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
                    using(core = new local_dbbmEntities1())
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
                using(core = new local_dbbmEntities1())
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
                using(core = new local_dbbmEntities1())
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
    }
}
