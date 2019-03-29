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
        public IHttpActionResult GetBlogs(int category = 0, int page = 0)
        {
            // create a root path...
            string path = Url.Content("~/Uploads/blog") + "/";
            // determine the number of blogs to skip. Take 8 for each page...
            int blogsToSkip = page * 8;
            // include both category and author of a blog...
            List<Blog> blogs = db.Blogs.Include("Category").Include("Author")
                               .Where(b => (category != 0 ? b.CategoryId == category : true)).OrderByDescending(b => b.Id)
                               .Skip(blogsToSkip).Take(8).ToList();
            // return what is needed...
            var blog = blogs.Select(b=>new {
                SmallPhoto = path + b.SmallPhoto,
                DetailsPhoto = path + b.DetailsPhoto,
                Date = b.Date.ToString("dd MMM, yyyy"),
                b.Title,
                b.Slug,
                b.Text,
                b.SpecialText,
                b.PostedBy,
                Category =  b.Category.Name,
                Author = new {
                    b.Author.Name,
                    Photo = path + b.Author.Photo
                }
            });
            return Ok(blog);
        }

        // blogs for blog-list view...
        [HttpGet]
        [Route("api/bloglist"), ActionName("GetBlogList")]
        public IHttpActionResult GetBlogList(string category = "", int page = 0)
        {
            string path = Url.Content("~/Uploads/blog") + "/";

            int categoryId;
            if (string.IsNullOrEmpty(category))
            {
                categoryId = 0;
            }
            else
            {
                if(db.Categories.Any(c=>c.Name.ToLower() == category.ToLower()))
                {
                    categoryId = db.Categories.FirstOrDefault(c => c.Name == category).Id;
                }
                else
                {
                    categoryId = 0;
                }
            }

            // determine the number of blogs to skip. Take 8 for each page...
            int blogsToSkip = (page - 1) * 8;

            if(blogsToSkip >= db.Blogs.Count())
            {
                return NotFound();
            }

            // include both category and author of a blog...
            List<Blog> blogs = db.Blogs.Include("Category").Include("Author")
                               .Where(b => (categoryId != 0 ? b.CategoryId == categoryId : true)).OrderByDescending(b => b.Id)
                               .Skip(blogsToSkip).Take(8).ToList();

            int pageCount = Convert.ToInt32(Math.Ceiling(blogs.Count() / 8.0));

            // return what is needed for view...
            var data = blogs.Select(b => new
            {
                Photo = path + b.SmallPhoto,
                b.Title,
                b.Slug,
                Author = new
                {
                    b.Author.Name,
                    Photo = path + b.Author.Photo
                },
                Date = b.Date.ToString("dd MMM, yyyy"),
                b.Desc,
                Category = b.Category.Name,
                PageCount = pageCount
            });

            return Ok(data);
        }

        // GET: api/Blogs/how-to-be-a-professional
        [ResponseType(typeof(Blog))]
        [Route("api/blogs/{slug}"), HttpGet, ActionName("OneBlog")]
        public IHttpActionResult GetBlog(string slug)
        {
            string path = Url.Content("~/Uploads/blog") + "/";
            Blog blog = db.Blogs.Include("Author").Include("Category").FirstOrDefault(b => b.Slug == slug);
            List<Blog> blogs = db.Blogs.OrderByDescending(b => b.Date).ToList();
            Blog nextBlog;
            Blog prevBlog;
            try
            {
                if((blogs.IndexOf(blog) + 1) < blogs.Count())
                {
                    nextBlog = blogs[blogs.IndexOf(blog) + 1];
                }
                else
                {
                    nextBlog = null;
                }
            }
            catch (KeyNotFoundException)
            {
                nextBlog = null;
            }
            try
            {
                if((blogs.IndexOf(blog) - 1) >= 0)
                {
                    prevBlog = blogs[blogs.IndexOf(blog) - 1];
                }
                else
                {
                    prevBlog = null;
                }
            }
            catch (KeyNotFoundException)
            {
                prevBlog = null;
            }
            if (blog == null)
            {
                return NotFound();
            }
            var data = new
            {
                blog.Id,
                Category = blog.Category.Name,
                blog.Title,
                blog.Text,
                blog.SpecialText,
                blog.PostedBy,
                Author = new
                {
                    blog.Author.Name,
                    Photo = path + blog.Author.Photo
                },
                blog.Slug,
                SmallPhoto = path + blog.SmallPhoto,
                DetailsPhoto = path + blog.DetailsPhoto,
                blog.Desc,
                DateDay = blog.Date.ToString("dd"),
                DateMonth = blog.Date.ToString("MMM"),
                NextSlug = nextBlog?.Slug,
                PrevSlug = prevBlog?.Slug
            };
            return Ok(data);
        }

        // blogs for news sliders...
        [HttpGet]
        [Route("api/getsliderblogs")]
        public IHttpActionResult GetSliderBlogs(int count = 3)
        {
            string path = Url.Content("~/Uploads/blog") + "/";
            // return what is needed for a slider...
            List<Blog> blogs = db.Blogs.Include("Author").Include("Category").OrderBy(b => Guid.NewGuid()).Take(count).ToList();
            var data = blogs.Select(b => new
            {
                b.Slug,
                b.Title,
                Photo = path + b.SmallPhoto,
                Author = new
                {
                    b.Author.Name,
                    Photo = path + b.Author.Photo
                },
                Category = b.Category.Name,
                Date = b.Date.ToString("dd MMM, yyyy"),
                b.Desc
            });

            return Ok(data);
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

        [HttpGet]
        [Route("api/pagenumber")]
        public IHttpActionResult GetPageDetails()
        {
            int PageCount = Convert.ToInt32(Math.Ceiling(db.Blogs.Count()/8.0));
            return Ok(PageCount);
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