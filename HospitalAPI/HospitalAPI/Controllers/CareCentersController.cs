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
    public class CareCentersController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/CareCenters
        public IQueryable<CareCenter> GetCareCenters()
        {
            return db.CareCenters;
        }

        // GET: api/CareCenters/5
        [ResponseType(typeof(CareCenter))]
        public IHttpActionResult GetCareCenter(int id)
        {
            CareCenter careCenter = db.CareCenters.Find(id);
            if (careCenter == null)
            {
                return NotFound();
            }

            return Ok(careCenter);
        }

        // PUT: api/CareCenters/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCareCenter(int id, CareCenter careCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != careCenter.Id)
            {
                return BadRequest();
            }

            db.Entry(careCenter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareCenterExists(id))
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

        private bool CareCenterExists(int id)
        {
            return db.CareCenters.Count(e => e.Id == id) > 0;
        }
    }
}