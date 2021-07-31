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

namespace capstone_backend.Controllers.Stock_on_hand
{
    [RoutePrefix("api/filter-stocks")]
    public class categoryByFilterStocksController : ApiController
    {
        private burgerdbEntities db = new burgerdbEntities();

        // GET: api/categoryByFilterStocks
        public IQueryable<tbcategory> Gettbcategories()
        {
            return db.tbcategories;
        }

        // GET: api/categoryByFilterStocks/5
        [Route("Filter-By-category-for-stocks"), HttpGet]
        public IHttpActionResult Gettbcategory(string categoryname)
        {
            using (db)
            {
                if(db.stock_on_hand.Any(x => x.productcategory == categoryname))
                {
                    var obj = db.stock_on_hand.Where(x => x.productcategory == categoryname).ToList();
                    return Ok(obj);
                }
                else
                {
                    return Ok("Category not exist");
                }
            }
        }

        // PUT: api/categoryByFilterStocks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbcategory(int id, tbcategory tbcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbcategory.id)
            {
                return BadRequest();
            }

            db.Entry(tbcategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbcategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/categoryByFilterStocks
        [ResponseType(typeof(tbcategory))]
        public IHttpActionResult Posttbcategory(tbcategory tbcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbcategories.Add(tbcategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbcategory.id }, tbcategory);
        }

        // DELETE: api/categoryByFilterStocks/5
        [ResponseType(typeof(tbcategory))]
        public IHttpActionResult Deletetbcategory(int id)
        {
            tbcategory tbcategory = db.tbcategories.Find(id);
            if (tbcategory == null)
            {
                return NotFound();
            }

            db.tbcategories.Remove(tbcategory);
            db.SaveChanges();

            return Ok(tbcategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbcategoryExists(int id)
        {
            return db.tbcategories.Count(e => e.id == id) > 0;
        }
    }
}