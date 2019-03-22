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
    public class FeedbacksController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Feedbacks
        public List<Feedback> GetFeedbacks()
        {
            string path = Url.Content("~/Uploads/patientAndAuthor") + "/";
            List<Feedback> feedbacks = db.Feedbacks.ToList();
            // add root path to each item...
            foreach (Feedback item in feedbacks)
            {
                item.ProfilePhoto = path + item.ProfilePhoto;
            }
            return feedbacks;
        }

        // GET: api/Feedbacks/5
        [ResponseType(typeof(Feedback))]
        public IHttpActionResult GetFeedback(int id)
        {
            string path = Url.Content("~/Uploads/patientAndAuthor") + "/";
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.ProfilePhoto = path + feedback.ProfilePhoto;
            return Ok(feedback);
        }

        // PUT: api/Feedbacks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeedback(int id, Feedback feedback)
        {
            #region Take the name of the old profile photo

            // find the department from the database, and take its name of photo...
            Feedback fb = db.Feedbacks.FirstOrDefault(f => f.Id == id);
            string oldPhoto;
            // check if it is null or not...
            if (fb != null)
            {
                oldPhoto = fb.ProfilePhoto;
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

            if (id != feedback.Id)
            {
                return BadRequest();
            }

            #endregion

            // deattach the old photo taking entry...
            db.Entry(fb).State = EntityState.Detached;

            // update another item...
            db.Entry(feedback).State = EntityState.Modified;

            // dealing with profile photo...
            #region Profile photo update

            if (string.IsNullOrEmpty(feedback.ProfilePhoto))
            {
                // if no photo comes, do NOT update the photo...
                db.Entry(feedback).Property(f => f.ProfilePhoto).IsModified = false;
            }
            else
            {
                // finding file extension...
                string photoExt = GetExtension.GetFileExtension(feedback.ProfilePhoto);

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
                        PhotoManager.Delete("patientAndAuthor", oldPhoto);
                        feedback.ProfilePhoto = PhotoManager.Upload(feedback.ProfilePhoto, photoExt, "patientAndAuthor");
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
                if (!FeedbackExists(id))
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

        // POST: api/Feedbacks
        [ResponseType(typeof(Feedback))]
        public IHttpActionResult PostFeedback(Feedback feedback)
        {
            // check the model, and if everything is OK, step to next level...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(feedback.ProfilePhoto))
            {
                ModelState.AddModelError("Photo", "Profile photo is required");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string photoExt = GetExtension.GetFileExtension(feedback.ProfilePhoto);

                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        feedback.ProfilePhoto = PhotoManager.Upload(feedback.ProfilePhoto, photoExt, "patientAndAuthor");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("FileConversion", "Base64 to photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feedback.Id }, feedback);
        }

        // DELETE: api/Feedbacks/5
        [ResponseType(typeof(Feedback))]
        public IHttpActionResult DeleteFeedback(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            // also deletethe photo...
            PhotoManager.Delete("patientAndAuthor", feedback.ProfilePhoto);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();

            return Ok(feedback);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedbackExists(int id)
        {
            return db.Feedbacks.Count(e => e.Id == id) > 0;
        }
    }
}