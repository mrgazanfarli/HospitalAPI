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
    public class DescCardsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/DescCards
        public List<DescCard> GetDescCards()
        {
            string path = Url.Content("~/Uploads") + "/";
            List<DescCard> descCards = db.DescCards.OrderByDescending(d => d.Id).ToList();
            // add the root path to all photos...
            foreach (DescCard item in descCards)
            {
                item.Photo = path + item.Photo;
            }
            return descCards;
        }

        // GET: api/DescCards/5
        [ResponseType(typeof(DescCard))]
        public IHttpActionResult GetDescCard(int id)
        {
            string path = Url.Content("~/Uploads") + "/";
            DescCard descCard = db.DescCards.Find(id);
            if (descCard == null)
            {
                return NotFound();
            }

            descCard.Photo = path + descCard.Photo;
            return Ok(descCard);
        }

        // PUT: api/DescCards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDescCard(int id, DescCard descCard)
        {
            #region Taking the name of old photo

            // find the descCard from the database, and take its name of photo...
            DescCard dc = db.DescCards.FirstOrDefault(d => d.Id == id);
            string oldPhoto;
            // check if it is null or not...
            if (dc != null)
            {
                oldPhoto = dc.Photo;
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

            if (id != descCard.Id)
            {
                return BadRequest();
            }

            #endregion

            // deattach the old descCard item as before...
            db.Entry(dc).State = EntityState.Detached;
            
            // update the item...
            db.Entry(descCard).State = EntityState.Modified;

            // dealing with photo...
            #region Photo update

            if (string.IsNullOrEmpty(descCard.Photo))
            {
                // if no photo comes, do NOT update the photo...
                db.Entry(descCard).Property(d => d.Photo).IsModified = false;
            }
            else
            {
                // finding file extension...
                string photoExt = GetExtension.GetFileExtension(descCard.Photo);

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
                        descCard.Photo = PhotoManager.Upload(descCard.Photo, photoExt, "");
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
                if (!DescCardExists(id))
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

        // POST: api/DescCards
        [ResponseType(typeof(DescCard))]
        public IHttpActionResult PostDescCard(DescCard descCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(descCard.Photo))
            {
                ModelState.AddModelError("Photo", "Card photo is required");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string photoExt = GetExtension.GetFileExtension(descCard.Photo);

                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("UnsupportedFileFormat", "File extension not correct!");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        descCard.Photo = PhotoManager.Upload(descCard.Photo, photoExt, "");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Conversion from base64 to photo failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }

            }

            db.DescCards.Add(descCard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = descCard.Id }, descCard);
        }

        // DELETE: api/DescCards/5
        [ResponseType(typeof(DescCard))]
        public IHttpActionResult DeleteDescCard(int id)
        {
            DescCard descCard = db.DescCards.Find(id);
            if (descCard == null)
            {
                return NotFound();
            }

            // delete the photo too...
            PhotoManager.Delete("", descCard.Photo);
            db.DescCards.Remove(descCard);
            db.SaveChanges();

            return Ok(descCard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DescCardExists(int id)
        {
            return db.DescCards.Count(e => e.Id == id) > 0;
        }
    }
}