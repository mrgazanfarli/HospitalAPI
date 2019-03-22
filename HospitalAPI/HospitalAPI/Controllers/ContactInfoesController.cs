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
    public class ContactInfoesController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/ContactInfoes
        public IQueryable<ContactInfo> GetContactInfos()
        {
            return db.ContactInfos;
        }

        // GET: api/ContactInfoes/5
        [ResponseType(typeof(ContactInfo))]
        public IHttpActionResult GetContactInfo(int id)
        {
            ContactInfo contactInfo = db.ContactInfos.Find(id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            return Ok(contactInfo);
        }

        // PUT: api/ContactInfoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContactInfo(int id, ContactInfo contactInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactInfo.Id)
            {
                return BadRequest();
            }

            db.Entry(contactInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactInfoExists(id))
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

        private bool ContactInfoExists(int id)
        {
            return db.ContactInfos.Count(e => e.Id == id) > 0;
        }
    }
}