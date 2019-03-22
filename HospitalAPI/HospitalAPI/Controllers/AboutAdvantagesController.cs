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
    public class AboutAdvantagesController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/AboutAdvantages
        public IQueryable<AboutAdvantage> GetAboutAdvantages()
        {
            return db.AboutAdvantages;
        }

        // GET: api/AboutAdvantages/5
        [ResponseType(typeof(AboutAdvantage))]
        public IHttpActionResult GetAboutAdvantage(int id)
        {
            AboutAdvantage aboutAdvantage = db.AboutAdvantages.Find(id);
            if (aboutAdvantage == null)
            {
                return NotFound();
            }

            return Ok(aboutAdvantage);
        }

        // PUT: api/AboutAdvantages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAboutAdvantage(int id, AboutAdvantage aboutAdvantage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aboutAdvantage.Id)
            {
                return BadRequest();
            }

            db.Entry(aboutAdvantage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutAdvantageExists(id))
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

        // POST: api/AboutAdvantages
        [ResponseType(typeof(AboutAdvantage))]
        public IHttpActionResult PostAboutAdvantage(AboutAdvantage aboutAdvantage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AboutAdvantages.Add(aboutAdvantage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aboutAdvantage.Id }, aboutAdvantage);
        }

        // DELETE: api/AboutAdvantages/5
        [ResponseType(typeof(AboutAdvantage))]
        public IHttpActionResult DeleteAboutAdvantage(int id)
        {
            AboutAdvantage aboutAdvantage = db.AboutAdvantages.Find(id);
            if (aboutAdvantage == null)
            {
                return NotFound();
            }

            db.AboutAdvantages.Remove(aboutAdvantage);
            db.SaveChanges();

            return Ok(aboutAdvantage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AboutAdvantageExists(int id)
        {
            return db.AboutAdvantages.Count(e => e.Id == id) > 0;
        }
    }
}