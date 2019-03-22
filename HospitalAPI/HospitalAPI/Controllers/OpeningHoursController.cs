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
    public class OpeningHoursController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/OpeningHours
        public IQueryable<OpeningHour> GetOpeningHours()
        {
            return db.OpeningHours;
        }

        // GET: api/OpeningHours/5
        [ResponseType(typeof(OpeningHour))]
        public IHttpActionResult GetOpeningHour(int id)
        {
            OpeningHour openingHour = db.OpeningHours.Find(id);
            if (openingHour == null)
            {
                return NotFound();
            }

            return Ok(openingHour);
        }

        // PUT: api/OpeningHours/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOpeningHour(int id, OpeningHour openingHour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != openingHour.Id)
            {
                return BadRequest();
            }

            db.Entry(openingHour).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpeningHourExists(id))
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

        // POST: api/OpeningHours
        [ResponseType(typeof(OpeningHour))]
        public IHttpActionResult PostOpeningHour(OpeningHour openingHour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OpeningHours.Add(openingHour);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = openingHour.Id }, openingHour);
        }

        // DELETE: api/OpeningHours/5
        [ResponseType(typeof(OpeningHour))]
        public IHttpActionResult DeleteOpeningHour(int id)
        {
            OpeningHour openingHour = db.OpeningHours.Find(id);
            if (openingHour == null)
            {
                return NotFound();
            }

            db.OpeningHours.Remove(openingHour);
            db.SaveChanges();

            return Ok(openingHour);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OpeningHourExists(int id)
        {
            return db.OpeningHours.Count(e => e.Id == id) > 0;
        }
    }
}