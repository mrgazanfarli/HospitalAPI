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
using HospitalAPI.Helpers;
using System.Web.Script.Serialization;

namespace HospitalAPI.Controllers
{
    public class AboutBodiesController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/AboutBodies
        public List<AboutBody> GetAboutBodies()
        {
            string path = Url.Content("~/Uploads") + "/";
            // in this case, there is only one item to show. Just find it and return...
            List<AboutBody> abs = db.AboutBodies.ToList();
            foreach (AboutBody item in abs)
            {
                item.Photo = path + item.Photo;
            }
            return abs;
            //AboutBody ab = db.AboutBodies.FirstOrDefault();
            //if (ab == null)
            //{
            //    return NotFound();
            //}
            //else
            //{

            //    // before returning, add the path to the photo...
            //    ab.Photo = path + ab.Photo;
            //    return Ok(ab);
            //}
        }

        // GET: api/AboutBodies/5
        [ResponseType(typeof(AboutBody))]
        public IHttpActionResult GetAboutBody(int id)
        {
            AboutBody aboutBody = db.AboutBodies.Find(id);
            if (aboutBody == null)
            {
                return NotFound();
            }
            aboutBody.Photo = Url.Content("~/Uploads") + "/" + aboutBody.Photo;
            return Ok(aboutBody);
        }

        // PUT: api/AboutBodies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAboutBody(int id, [FromBody] AboutBody aboutBody)
        {
            #region Take old photo name
            // Find the item and then take the old photo value to delete it later...
            AboutBody ab = db.AboutBodies.FirstOrDefault(s => s.Id == id);
            string oldPhoto;
            // Check if it is null or not, for avoiding real-time error later...
            if (ab == null)
            {
                return NotFound();
            }
            else
            {
                oldPhoto = ab.Photo;
            }
            #endregion

            #region Validations
            // Check if the validations are OK or not...
            if (id != aboutBody.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            // Deattach the first-created aboutbody, because it will give an error for having the same 2 primary keys...
            db.Entry(ab).State = EntityState.Detached;

            // Update another entry...
            db.Entry(aboutBody).State = EntityState.Modified;

            // Check for photo validations...
            #region Updating the photo
            if (string.IsNullOrEmpty(aboutBody.Photo))
            {
                // if the photo is null, do NOT update it...
                db.Entry(aboutBody).Property(a => a.Photo).IsModified = false;
            }
            else
            {
                // find image format...
                string photoExt = GetExtension.GetFileExtension(aboutBody.Photo);

                // if empty, return error...
                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("IncorrectFileExtension", "File extension not correct");
                }
                else
                {
                    try
                    {
                        PhotoManager.Delete("", oldPhoto);
                        aboutBody.Photo = PhotoManager.Upload(aboutBody.Photo, photoExt, "");
                    }
                    // if cannot upload the photo, return its error...
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }
            #endregion

            // if all OK, update the database...
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutBodyExists(id))
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

        // POST: api/AboutBodies
        [ResponseType(typeof(AboutBody))]
        public IHttpActionResult PostAboutBody(AboutBody aboutBody)
        {
            // check the model, and if everything is OK, add it to database...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(aboutBody.Photo))
            {
                ModelState.AddModelError("Photo", "Photo is required");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string PhotoExt = GetExtension.GetFileExtension(aboutBody.Photo);

                if (string.IsNullOrEmpty(PhotoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension is incorrect");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        aboutBody.Photo = PhotoManager.Upload(aboutBody.Photo, PhotoExt, "");
                    }
                    catch(InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Conversion of base64 failed, try again");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.AboutBodies.Add(aboutBody);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aboutBody.Id }, aboutBody);
        }

        // DELETE: api/AboutBodies/5
        [ResponseType(typeof(AboutBody))]
        public IHttpActionResult DeleteAboutBody(int id)
        {
            AboutBody aboutBody = db.AboutBodies.Find(id);
            if (aboutBody == null)
            {
                return NotFound();
            }

            PhotoManager.Delete("", aboutBody.Photo);
            db.AboutBodies.Remove(aboutBody);
            db.SaveChanges();

            return Ok(aboutBody);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AboutBodyExists(int id)
        {
            return db.AboutBodies.Count(e => e.Id == id) > 0;
        }
    }
}