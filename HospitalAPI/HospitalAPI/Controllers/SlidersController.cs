using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalAPI.DAL;
using HospitalAPI.Models;
using HospitalAPI.Helpers;

namespace HospitalAPI.Controllers
{
    public class SlidersController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Sliders
        public List<Slider> GetSliders()
        {
            // returning all items in the DB...
            string path = Url.Content("~/Uploads/slider") + "/";
            List<Slider> sliders = db.Sliders.ToList();
            foreach (Slider item in sliders)
            {
                item.BackImage = path + item.BackImage;
            }
            return sliders;
            // this method did not work, because of serialization problem...
            //return db.Sliders.Select(s => new
            //{
            //    s.Id,
            //    s.Desc,
            //    BackImage = path + s.BackImage
            //});
        }

        // GET: api/Sliders/5
        [ResponseType(typeof(Slider))]
        public IHttpActionResult GetSlider(int id)
        {
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return NotFound();
            }

            // before returning, add the path to the photo...
            slider.BackImage = Url.Content("~/Uploads/slider") + "/" + slider.BackImage;
            return Ok(slider);
        }

        // PUT: api/Sliders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSlider(int id, [FromBody] Slider slider)
        {
            #region Take old photo details
            // Find the item and then take the old photo value to delete it later...
            Slider sl = db.Sliders.FirstOrDefault(s => s.Id == id);
            string oldPhoto;
            // Check if it is null or not, for avoiding real-time error later...
            if (sl != null)
            {
                oldPhoto = sl.BackImage;
            }
            else
            {
                return NotFound();
            }
            #endregion

            #region CheckPresenceAndModelState
            if (id != slider.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #endregion

            // Deattach the first-created slider, because it will give an error for having the same 2 primary keys...
            db.Entry(sl).State = EntityState.Detached;

            // Update another entry...
            db.Entry(slider).State = EntityState.Modified;

            // Check if the photo validations are OK or not...
            #region Photo Updating
            if (string.IsNullOrEmpty(slider.BackImage))
            {
                // if the photo is null, do NOT update it...
                db.Entry(slider).Property(s => s.BackImage).IsModified = false;
            }
            else
            {
                // find data format...
                string PhotoExt = GetExtension.GetFileExtension(slider.BackImage);

                // if empty, return error...
                if (string.IsNullOrEmpty(PhotoExt))
                {
                    ModelState.AddModelError("IncorrectFileExtension", "File extension not correct!");
                }
                else
                {
                    try
                    {
                        PhotoManager.Delete("slider", oldPhoto);
                        slider.BackImage = PhotoManager.Upload(slider.BackImage, PhotoExt, "slider");
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
                if (!SliderExists(id))
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

        // POST: api/Sliders
        [ResponseType(typeof(Slider))]
        public IHttpActionResult PostSlider(Slider slider)
        {
            // check the model, and if everything is OK, add it to database...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(slider.BackImage))
            {
                ModelState.AddModelError("Photo", "Background Image is required");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string PhotoExt = GetExtension.GetFileExtension(slider.BackImage);

                if (string.IsNullOrEmpty(PhotoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct!");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        slider.BackImage = PhotoManager.Upload(slider.BackImage, PhotoExt, "slider");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.Sliders.Add(slider);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = slider.Id }, slider);
        }


        //[HttpPost]
        //[Route("api/elcin")]
        //public IHttpActionResult Elcin(HttpRequestMessage Request)
        //{
        //    foreach (var item in HttpContext.Current.Request.Form.AllKeys.SelectMany(HttpContext.Current.Request.Form.GetValues,(k,v) => new { key = k, value = v }))
        //    {
        //        var data = item.value;
        //    }

        //    HttpPostedFile Photo = HttpContext.Current.Request.Files[0];
        //    return Ok();
        //}

        // DELETE: api/Sliders/5
        [ResponseType(typeof(Slider))]
        public IHttpActionResult DeleteSlider(int id)
        {
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return NotFound();
            }
            // delete its photo too...
            PhotoManager.Delete("slider", slider.BackImage);
            db.Sliders.Remove(slider);
            db.SaveChanges();

            return Ok(slider);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SliderExists(int id)
        {
            return db.Sliders.Count(e => e.Id == id) > 0;
        }
    }
}