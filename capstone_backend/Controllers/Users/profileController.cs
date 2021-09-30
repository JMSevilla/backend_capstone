using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.Users
{
    [RoutePrefix("api/get-details-profile")]
    public class profileController : ApiController
    {
        private local_dbbmEntities2 db = new local_dbbmEntities2();

        // GET: api/profile
        public IQueryable<user> Getusers()
        {
            return db.users;
        }

        // GET: api/profile/5
        [Route("profile-catcher"), HttpGet]
        public IHttpActionResult Getuser(string email)
        {
            try
            {
                using (db)
                {
                    if(db.users.Any(x => x.email == email))
                    {
                        var obj = db.users.Where(x => x.email == email).ToList();
                        return Ok(obj);
                    }
                    else
                    {
                        return Ok("not exist");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        APISecurity apisecure = new APISecurity();
        // PUT: api/profile/5
        [Route("update-profile"), HttpPut]
        public IHttpActionResult Putuser(int id)
        {
            try
            {
                var http = HttpContext.Current.Request;
                if (id <= 0)
                {
                    return Ok("invalid id");
                }
                else
                {
                    using (db)
                    {
                        var obj = db.users.Where(x => x.id == id).FirstOrDefault();
                        if(obj != null)
                        {
                            if (string.IsNullOrEmpty(http.Form["password"]))
                            {
                                obj.firstname = http.Form["firstname"];
                                obj.lastname = http.Form["lastname"];
                                obj.email = http.Form["email"];
                                obj.imageurl = http.Form["image"];
                                db.SaveChanges();
                                return Ok("success");
                            }
                            else
                            {
                                obj.firstname = http.Form["firstname"];
                                obj.lastname = http.Form["lastname"];
                                obj.email = http.Form["email"];
                                obj.imageurl = http.Form["image"];
                                obj.password = apisecure.Encrypt(http.Form["password"]);
                                db.SaveChanges();
                                return Ok("success");
                            }
                        }
                        else
                        {
                            return Ok("invalid id not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/profile
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.id }, user);
        }

        // DELETE: api/profile/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(int id)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.users.Count(e => e.id == id) > 0;
        }
    }
}