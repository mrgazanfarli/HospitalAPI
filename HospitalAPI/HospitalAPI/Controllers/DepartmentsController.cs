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
    public class DepartmentsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Departments
        public List<Department> GetDepartments()
        {
            string path = Url.Content("~/Uploads/department") + "/";
            List<Department> departments = db.Departments.ToList();
            foreach (Department item in departments)
            {
                item.Photo = path + item.Photo;
            }

            return departments;
        }

        [HttpGet]
        [Route("api/getdepartmentnames")]
        public IHttpActionResult GetDepartmentNames()
        {
            var names = db.Departments.OrderBy(d => Guid.NewGuid()).Select(d => new
            {
                d.Name,
                d.Slug
            });

            if (names != null)
            {
                return Ok(names);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Departments/5
        [ResponseType(typeof(Department))]
        public IHttpActionResult GetDepartment(string slug)
        {
            string path = Url.Content("~/Uploads/department") + "/";
            Department department = db.Departments.FirstOrDefault(d => d.Slug == slug);
            if (department == null)
            {
                return NotFound();
            }

            department.Photo = path + department.Photo;
            return Ok(department);
        }

        // PUT: api/Departments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepartment(int id, Department department)
        {
            #region Take the name of the old photo

            // find the department from the database, and take its name of photo...
            Department dep = db.Departments.FirstOrDefault(d => d.Id == id);
            string oldPhoto;
            // check if it is null or not...
            if (dep != null)
            {
                oldPhoto = dep.Photo;
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

            if (id != department.Id)
            {
                return BadRequest();
            }

            #endregion

            // Check if the slug exists or not...
            if(db.Departments.Any(d=>d.Slug == department.Slug))
            {
                ModelState.AddModelError("Slug", "Slug already exists");
                return StatusCode(HttpStatusCode.Conflict);
            }

            // as the old photo name was taken, it is required to deattach "dep",
            // because it will be error to work with the same 2 primary keys...
            db.Entry(dep).State = EntityState.Detached;

            // update another item...
            db.Entry(department).State = EntityState.Modified;

            // dealing with photo...
            #region Photo update

            if (string.IsNullOrEmpty(department.Photo))
            {
                // if no photo comes, do NOT update the photo...
                db.Entry(department).Property(d => d.Photo).IsModified = false;
            }
            else
            {
                // finding file extension...
                string photoExt = GetExtension.GetFileExtension(department.Photo);

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
                        PhotoManager.Delete("department", oldPhoto);
                        department.Photo = PhotoManager.Upload(department.Photo, photoExt, "department");
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
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        [ResponseType(typeof(Department))]
        public IHttpActionResult PostDepartment(Department department)
        {
            // check the model, and if everything is OK, step to next level...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the slug exists or not...
            if (db.Departments.Any(d => d.Slug == department.Slug))
            {
                ModelState.AddModelError("Slug", "Slug already exists");
                return StatusCode(HttpStatusCode.Conflict);
            }

            // Manually check if the photo is null or not...
            if (string.IsNullOrEmpty(department.Photo))
            {
                ModelState.AddModelError("Photo", "Department photo is required");
            }
            else
            {
                // if all OK, find the extension of file. If it is empty, it means it is unsupported media type
                string photoExt = GetExtension.GetFileExtension(department.Photo);

                if (string.IsNullOrEmpty(photoExt))
                {
                    ModelState.AddModelError("BadFileExtension", "File extension not correct");
                }
                else
                {
                    // try to convert photo from base64 to an image. If any problems, return media file issue...
                    try
                    {
                        department.Photo = PhotoManager.Upload(department.Photo, photoExt, "department");
                    }
                    catch (InvalidCastException)
                    {
                        ModelState.AddModelError("FileConversion", "Base64 to photo conversion failed");
                        return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    }
                }
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            // delete its photo too...
            PhotoManager.Delete("department", department.Photo);
            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.Id == id) > 0;
        }
    }
}