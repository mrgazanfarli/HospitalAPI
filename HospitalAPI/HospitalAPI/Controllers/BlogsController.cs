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
    public class BlogsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Blogs
        [HttpGet]
        [Route("api/blogs")]
        public List<Blog> GetBlogs(int category = 0, int page = 0)
        {
            // create a root path...
            string path = Url.Content("~/Uploads/blog") + "/";
            // determine the number of blogs to skip. Take 8 for each page...
            int blogsToSkip = page * 8;
            // include both category and author of a blog...
            List<Blog> blogs = db.Blogs.Include("Category").Include("Author")
                               .Where(b => (category != 0 ? b.CategoryId == category : true)).OrderByDescending(b => b.Id)
                               .Skip(blogsToSkip).Take(8).ToList();
            // add root path to the blog images...
            foreach (Blog item in blogs)
            {
                item.SmallPhoto = path + item.SmallPhoto;
                item.DetailsPhoto = path + item.DetailsPhoto;
            }

            // and then, return the blogs taken...
            return blogs;
        }

        // GET: api/Blogs/5
        [ResponseType(typeof(Blog))]
        public IHttpActionResult GetBlog(string slug)
        {
            string path = Url.Content("~/Uploads/blog") + "/";
            Blog blog = db.Blogs.FirstOrDefault(b => b.Slug == slug);
            if (blog == null)
            {
                return NotFound();
            }

            blog.SmallPhoto = path + blog.SmallPhoto;
            blog.DetailsPhoto = path + blog.DetailsPhoto;
            return Ok(blog);
        }

        // PUT: api/Blogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlog(int id, Blog blog)
        {
            #region Take the names of the old photos

            // firstly, old photos' name to use them in deleting process...
            Blog bl = db.Blogs.FirstOrDefault(b => b.Id == id);
            string oldSmallPhoto, oldDetPhoto;
            // check if the blog is null or not...
            if (bl != null)
            {
                // if it exists, take values you need...
                oldSmallPhoto = bl.SmallPhoto;
                oldDetPhoto = bl.DetailsPhoto;
            }
            else
            {
                return NotFound();
            }

            #endregion

            #region ID and Model checking

            if (id != blog.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #endregion

            // check if the slug is OK or not...
            if(db.Blogs.Any(b=>b.Slug == blog.Slug))
            {
                ModelState.AddModelError("Slug", "The slug already exists");
                return StatusCode(HttpStatusCode.Conflict);
            }

            // after taking the old photo names, we have to deattach this model, because working with same two primary keys will cause an error...
            db.Entry(bl).State = EntityState.Detached;

            // update another blog...
            db.Entry(blog).State = EntityState.Modified;

            db.Entry(blog).Property(b => b.Date).IsModified = false;

            // check if the photos meet the requirements...
            #region Photo updating for small photo of the blog

            // next, check the photos, and if they are null, do NOT update them... repeat the process for both photos samely...
            // if any photo is not null, try to find its extension as a file, and then upload it to the server and update the DB...
            // delete the old photos from the server too...
            if (string.IsNullOrEmpty(blog.SmallPhoto))
            {
                db.Entry(blog).Property(b => b.SmallPhoto).IsModified = false;
            }
            else
            {
                string smallPhotoExt = GetExtension.GetFileExtension(blog.SmallPhoto);

                // if no extension, return error in modelstate...
                if (string.IsNullOrEmpty(smallPhotoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct");
                }
                else
                {
                    try
                    {
                        PhotoManager.Delete("blog", oldSmallPhoto);
                        blog.SmallPhoto = PhotoManager.Upload(blog.SmallPhoto, smallPhotoExt, "blog");
                    }
                    // return error on any problem with deleting or uploading...
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 to photo conversion failed...");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            #endregion

            #region Photo updating for detailed photo of the blog

            // repeat the same procedure as the small photo case...
            if (string.IsNullOrEmpty(blog.DetailsPhoto))
            {
                db.Entry(blog).Property(b => b.DetailsPhoto).IsModified = false;
            }
            else
            {
                string detPhotoExt = GetExtension.GetFileExtension(blog.DetailsPhoto);

                if (string.IsNullOrEmpty(detPhotoExt))
                {
                    ModelState.AddModelError("BadFileFormat", "File extension not correct");
                }
                else
                {
                    try
                    {
                        PhotoManager.Delete("blog", oldDetPhoto);
                        blog.DetailsPhoto = PhotoManager.Upload(blog.DetailsPhoto, detPhotoExt, "blog");
                    }
                    catch (InvalidExpressionException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 to photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            #endregion

            // if all OK, try to update the database...
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
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

        // POST: api/Blogs
        [HttpPost]
        [ResponseType(typeof(Blog))]
        [Route("api/blogs"), ActionName("PostBlog")]
        public IHttpActionResult PostBlog(Blog blog)
        {
            blog.Date = DateTime.Now;

            // check if the slug is OK or not...
            if (db.Blogs.Any(b => b.Slug == blog.Slug))
            {
                ModelState.AddModelError("Slug", "The slug already exists");
                return StatusCode(HttpStatusCode.Conflict);
            }

            // after media processes, check the whole model again and continue...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // manually check if the blog photos are null or not...
            if (string.IsNullOrEmpty(blog.SmallPhoto) || string.IsNullOrEmpty(blog.DetailsPhoto))
            {
                ModelState.AddModelError("Photo", "Blog photos are required");
            }
            else
            {
                // if the photos are available, find their extension...
                string smallPhotoExt = GetExtension.GetFileExtension(blog.SmallPhoto);
                string detPhotoExt = GetExtension.GetFileExtension(blog.DetailsPhoto);

                // if extension not found, make an error for that...
                if(string.IsNullOrEmpty(smallPhotoExt) || string.IsNullOrEmpty(detPhotoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct!");
                }
                else
                {
                    // try to convert the photos from base64, to images. On any problem, return photo issue...
                    try
                    {
                        blog.SmallPhoto = PhotoManager.Upload(blog.SmallPhoto, smallPhotoExt, "blog");
                        blog.DetailsPhoto = PhotoManager.Upload(blog.DetailsPhoto, detPhotoExt, "blog");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("PhotoConversion", "Base64 to image conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.Blogs.Add(blog);
            db.SaveChanges();

            return CreatedAtRoute("PostBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blogs/5
        [ResponseType(typeof(Blog))]
        public IHttpActionResult DeleteBlog(int id)
        {
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            db.Blogs.Remove(blog);
            db.SaveChanges();

            return Ok(blog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlogExists(int id)
        {
            return db.Blogs.Count(e => e.Id == id) > 0;
        }
    }
}