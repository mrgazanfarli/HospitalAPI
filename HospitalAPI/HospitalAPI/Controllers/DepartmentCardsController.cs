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
    public class DepartmentCardsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/DepartmentCards
        public IQueryable<DepartmentCard> GetDepartmentCards()
        {
            return db.DepartmentCards;
        }

        // GET: api/DepartmentCards/5
        [ResponseType(typeof(DepartmentCard))]
        public IHttpActionResult GetDepartmentCard(int id)
        {
            DepartmentCard departmentCard = db.DepartmentCards.Find(id);
            if (departmentCard == null)
            {
                return NotFound();
            }

            return Ok(departmentCard);
        }

        // PUT: api/DepartmentCards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepartmentCard(int id, DepartmentCard departmentCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != departmentCard.Id)
            {
                return BadRequest();
            }

            db.Entry(departmentCard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentCardExists(id))
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

        // POST: api/DepartmentCards
        [ResponseType(typeof(DepartmentCard))]
        public IHttpActionResult PostDepartmentCard(DepartmentCard departmentCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DepartmentCards.Add(departmentCard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = departmentCard.Id }, departmentCard);
        }

        // DELETE: api/DepartmentCards/5
        [ResponseType(typeof(DepartmentCard))]
        public IHttpActionResult DeleteDepartmentCard(int id)
        {
            DepartmentCard departmentCard = db.DepartmentCards.Find(id);
            if (departmentCard == null)
            {
                return NotFound();
            }

            db.DepartmentCards.Remove(departmentCard);
            db.SaveChanges();

            return Ok(departmentCard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentCardExists(int id)
        {
            return db.DepartmentCards.Count(e => e.Id == id) > 0;
        }
    }
}