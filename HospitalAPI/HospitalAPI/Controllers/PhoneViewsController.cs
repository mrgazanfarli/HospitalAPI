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
    public class PhoneViewsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/PhoneViews
        [HttpGet]
        [Route("api/phoneview")]
        public PhoneView GetPhoneView()
        {
            string path = Url.Content("~/Uploads") + "/";
            PhoneView phoneView = db.PhoneViews.FirstOrDefault();
            phoneView.Photo = path + phoneView.Photo;
            return phoneView;
        }

        // PUT: api/PhoneViews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPhoneView(int id, PhoneView phoneView)
        {
            #region Take the name of the old photo

            // find the department from the database, and take its name of photo...
            PhoneView pv = db.PhoneViews.FirstOrDefault(p => p.Id == id);
            string oldPhoto;
            // check if it is null or not...
            if (pv != null)
            {
                oldPhoto = pv.Photo;
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

            if (id != phoneView.Id)
            {
                return BadRequest();
            }

            #endregion

            // deattach the first entry...
            db.Entry(pv).State = EntityState.Detached;

            // update the using one...
            db.Entry(phoneView).State = EntityState.Modified;

            // dealing with photo...
            #region Photo update

            if (string.IsNullOrEmpty(phoneView.Photo))
            {
                // if no photo comes, do NOT update the photo...
                db.Entry(phoneView).Property(p => p.Photo).IsModified = false;
            }
            else
            {
                // finding file extension...
                string photoExt = GetExtension.GetFileExtension(phoneView.Photo);

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
                        PhotoManager.Delete("", oldPhoto);
                        phoneView.Photo = PhotoManager.Upload(phoneView.Photo, photoExt, "");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 to photo conversion failed");
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
                if (!PhoneViewExists(id))
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

        #region Posting

        //// POST: api/PhoneViews
        //[ResponseType(typeof(PhoneView))]
        //public IHttpActionResult PostPhoneView(PhoneView phoneView)
        //{
        //    // check the model, and if everything is OK, step to next level...
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Manually check if the photo is null or not...
        //    if (string.IsNullOrEmpty(phoneView.Photo))
        //    {
        //        ModelState.AddModelError("Photo", "Phone view photo is required");
        //    }
        //    else
        //    {
        //        // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
        //        string photoExt = GetExtension.GetFileExtension(phoneView.Photo);

        //        if (string.IsNullOrEmpty(photoExt))
        //        {
        //            ModelState.AddModelError("BadFileExtension", "File extension not correct");
        //        }
        //        else
        //        {
        //            // try to convert photo from base64 to an image. If any problems, return media file issue...
        //            try
        //            {
        //                phoneView.Photo = PhotoManager.Upload(phoneView.Photo, photoExt, "");
        //            }
        //            catch (InvalidCastException)
        //            {
        //                ModelState.AddModelError("FileConversion", "Base64 to photo conversion failed");
        //                return StatusCode(HttpStatusCode.UnsupportedMediaType);
        //            }
        //        }
        //    }


        //    db.PhoneViews.Add(phoneView);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = phoneView.Id }, phoneView);
        //}

        #endregion

        #region Deleting

        // DELETE: api/PhoneViews/5
        //[ResponseType(typeof(PhoneView))]
        //public IHttpActionResult DeletePhoneView(int id)
        //{
        //    PhoneView phoneView = db.PhoneViews.Find(id);
        //    if (phoneView == null)
        //    {
        //        return NotFound();
        //    }

        //    db.PhoneViews.Remove(phoneView);
        //    db.SaveChanges();

        //    return Ok(phoneView);
        //}

        #endregion
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneViewExists(int id)
        {
            return db.PhoneViews.Count(e => e.Id == id) > 0;
        }
    }
}