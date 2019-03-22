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
    public class AuthorsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Authors
        public List<Author> GetAuthors()
        {
            // returning all items in the DB...
            string path = Url.Content("~/Uploads/patientAndAuthor") + "/";
            List<Author> authors = db.Authors.ToList();
            foreach (Author item in authors)
            {
                item.Photo = path + item.Photo;
            }
            return authors;
        }

        // GET: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult GetAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            // before returning, add the path to the photo...
            author.Photo = Url.Content("~/Uploads/patientAndAuthor") + "/" + author.Photo;
            return Ok(author);
        }

        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAuthor(int id, Author author)
        {
            #region Take Old Photo's Name

            // Find the item and then take the old photo value to delete it later...
            Author ath = db.Authors.FirstOrDefault(a => a.Id == id);
            string oldPhoto;
            // check if the id is correct or not, and the "ath" has any value or not...
            if (ath != null)
            {
                // if author is found, take the old photo name...
                oldPhoto = ath.Photo;
            }
            else
            {
                // it is not found, return NotFound...
                return NotFound();
            }

            #endregion

            #region ID Checking...
            // check the id and return BadRequest if there is any problem with it...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.Id)
            {
                return BadRequest();
            }

            #endregion

            // after taking the old photo's name, deattach the first found author, because it will cause confusing problems when dealing with two same primary keys...
            db.Entry(ath).State = EntityState.Detached;

            // update another entry...
            db.Entry(author).State = EntityState.Modified;

            // after taking the photo, try to convert it from base64 and upload to the server, while deleting the old one...
            #region Photo Processing...

            // check if the photo is null or not...
            if (string.IsNullOrEmpty(author.Photo))
            {
                // if the photo is null, do NOT update it...
                db.Entry(author).Property(a => a.Photo).IsModified = false;
            }
            else
            {
                // find data format...
                string photoExt = GetExtension.GetFileExtension(author.Photo);

                // if empty, return error...
                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct!");
                }
                else
                {
                    try
                    {
                        PhotoManager.Delete("patientAndAuthor", oldPhoto);
                        author.Photo = PhotoManager.Upload(author.Photo, photoExt, "patientAndAuthor");
                    }
                    // if cannot upload the photo, return its error...
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 photo conversion failed");
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
                if (!AuthorExists(id))
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

        // POST: api/Authors
        [ResponseType(typeof(Author))]
        public IHttpActionResult PostAuthor(Author author)
        {
            // check the model, and if everything is OK, add it to database...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(author.Photo))
            {
                ModelState.AddModelError("Photo", "Author photo is required!");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string photoExt = GetExtension.GetFileExtension(author.Photo);

                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File format not correct!");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        author.Photo = PhotoManager.Upload(author.Photo, photoExt, "patientAndAuthor");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.Authors.Add(author);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            // delete its photo too...
            PhotoManager.Delete("patientAndAuthor", author.Photo);
            db.Authors.Remove(author);
            db.SaveChanges();

            return Ok(author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Count(e => e.Id == id) > 0;
        }
    }
}