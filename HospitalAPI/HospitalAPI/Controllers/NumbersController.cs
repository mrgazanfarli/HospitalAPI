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
using HospitalAPI.DAL;
using HospitalAPI.Models;

namespace HospitalAPI.Controllers
{
    public class NumbersController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Numbers
        public object GetNumbers()
        {
            Number number = db.Numbers.FirstOrDefault();
            if(number != null)
            {
                return number;
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/Numbers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNumber(int id, Number number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != number.Id)
            {
                return BadRequest();
            }

            db.Entry(number).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NumberExists(id))
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NumberExists(int id)
        {
            return db.Numbers.Count(e => e.Id == id) > 0;
        }
    }
}