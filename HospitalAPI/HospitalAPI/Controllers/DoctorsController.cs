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
using HospitalAPI.Helpers;
using HospitalAPI.Models;

namespace HospitalAPI.Controllers
{
    public class DoctorsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Doctors
        public List<Doctor> GetDoctors()
        {
            string path = Url.Content("~/Uploads/doctors") + "/";
            List<Doctor> doctors = db.Doctors.ToList();
            foreach (Doctor item in doctors)
            {
                item.Photo = path + item.Photo;
            }

            return doctors;
        }

        // GET: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult GetDoctor(string slug)
        {
            string path = Url.Content("~/Uploads/department") + "/";
            Doctor doctor = db.Doctors.FirstOrDefault(d => d.Slug == slug);
            if (doctor == null)
            {
                return NotFound();
            }

            doctor.Photo = path + doctor.Photo;
            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, Doctor doctor)
        {
            #region Take the name of the old photo

            // find the doctor from the database, and take its name of photo...
            Doctor doc = db.Doctors.FirstOrDefault(d => d.Id == id);
            string oldPhoto;
            // check if it is null or not...
            if (doc != null)
            {
                oldPhoto = doc.Photo;
            }
            else
            {
                return NotFound();
            }

            #endregion

            #region Check ID and model state

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.Id)
            {
                return BadRequest();
            }

            #endregion

            // Check if the slug exists or not...
            if (db.Doctors.Any(d => d.Slug == doctor.Slug))
            {
                ModelState.AddModelError("Slug", "Slug already exists");
                return StatusCode(HttpStatusCode.Conflict);
            }

            // deattach the item used for taking old photo's name. It will cause an error using two same primary keys...
            db.Entry(doc).State = EntityState.Detached;

            // update the entry...
            db.Entry(doctor).State = EntityState.Modified;

            // update the photo...
            #region Photo update

            if (string.IsNullOrEmpty(doctor.Photo))
            {
                // if no photo comes, do NOT update the photo...
                db.Entry(doctor).Property(d => d.Photo).IsModified = false;
            }
            else
            {
                // finding file extension...
                string photoExt = GetExtension.GetFileExtension(doctor.Photo);

                // return error if no extension found...
                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("UnsupportedFileExtension", "File extension not correct");
                }
                else
                {
                    // if the file format is OK, try to upload it to the server and DB...
                    try
                    {
                        PhotoManager.Delete("doctors", oldPhoto);
                        doctor.Photo = PhotoManager.Upload(doctor.Photo, photoExt, "doctors");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 to photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            #endregion

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        // POST: api/Doctors
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult PostDoctor(Doctor doctor)
        {
            // check the model, and if everything is OK, go to next steps...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the slug exists or not...
            if (db.Doctors.Any(d => d.Slug == doctor.Slug))
            {
                ModelState.AddModelError("Slug", "Slug already exists");
                return StatusCode(HttpStatusCode.Conflict);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(doctor.Photo))
            {
                ModelState.AddModelError("Photo", "Doctor's photo is required");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string photoExt = GetExtension.GetFileExtension(doctor.Photo);

                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        doctor.Photo = PhotoManager.Upload(doctor.Photo, photoExt, "doctors");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("FileConversion", "Base64 to photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.Id }, doctor);
        }

        // DELETE: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            // also delete the photo...
            PhotoManager.Delete("doctors", doctor.Photo);
            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.Id == id) > 0;
        }
    }
}