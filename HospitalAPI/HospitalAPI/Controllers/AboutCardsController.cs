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
    public class AboutCardsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/AboutCards
        public IQueryable<AboutCard> GetAboutCards()
        {
            return db.AboutCards;
        }

        // GET: api/AboutCards/5
        [ResponseType(typeof(AboutCard))]
        public IHttpActionResult GetAboutCard(int id)
        {
            AboutCard aboutCard = db.AboutCards.Find(id);
            if (aboutCard == null)
            {
                return NotFound();
            }

            return Ok(aboutCard);
        }

        // PUT: api/AboutCards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAboutCard(int id, AboutCard aboutCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aboutCard.Id)
            {
                return BadRequest();
            }

            db.Entry(aboutCard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutCardExists(id))
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

        // POST: api/AboutCards
        [ResponseType(typeof(AboutCard))]
        public IHttpActionResult PostAboutCard(AboutCard aboutCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AboutCards.Add(aboutCard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aboutCard.Id }, aboutCard);
        }

        // DELETE: api/AboutCards/5
        [ResponseType(typeof(AboutCard))]
        public IHttpActionResult DeleteAboutCard(int id)
        {
            AboutCard aboutCard = db.AboutCards.Find(id);
            if (aboutCard == null)
            {
                return NotFound();
            }

            db.AboutCards.Remove(aboutCard);
            db.SaveChanges();

            return Ok(aboutCard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AboutCardExists(int id)
        {
            return db.AboutCards.Count(e => e.Id == id) > 0;
        }
    }
}