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
    public class AboutIntroesController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/AboutIntroes
        public IQueryable<AboutIntro> GetAboutIntros()
        {
            return db.AboutIntros;
        }

        // GET: api/AboutIntroes/5
        [ResponseType(typeof(AboutIntro))]
        public IHttpActionResult GetAboutIntro(int id)
        {
            AboutIntro aboutIntro = db.AboutIntros.Find(id);
            if (aboutIntro == null)
            {
                return NotFound();
            }

            return Ok(aboutIntro);
        }

        // PUT: api/AboutIntroes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAboutIntro(int id, AboutIntro aboutIntro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aboutIntro.Id)
            {
                return BadRequest();
            }

            db.Entry(aboutIntro).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutIntroExists(id))
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

        // POST: api/AboutIntroes
        [ResponseType(typeof(AboutIntro))]
        public IHttpActionResult PostAboutIntro(AboutIntro aboutIntro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AboutIntros.Add(aboutIntro);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aboutIntro.Id }, aboutIntro);
        }

        // DELETE: api/AboutIntroes/5
        [ResponseType(typeof(AboutIntro))]
        public IHttpActionResult DeleteAboutIntro(int id)
        {
            AboutIntro aboutIntro = db.AboutIntros.Find(id);
            if (aboutIntro == null)
            {
                return NotFound();
            }

            db.AboutIntros.Remove(aboutIntro);
            db.SaveChanges();

            return Ok(aboutIntro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AboutIntroExists(int id)
        {
            return db.AboutIntros.Count(e => e.Id == id) > 0;
        }
    }
}