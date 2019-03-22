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

namespace HospitalAPI.Controllers
{
    public class QuestionsController : ApiController
    {
        private HospitalContext db = new HospitalContext();

        // GET: api/Questions
        public IQueryable<Question> GetQuestions()
        {
            return db.Questions;
        }

        // Get hidden questions: api/hiddenquestions
        [HttpGet]
        [Route("api/hiddenquestions")]
        public IQueryable<Question> GetHiddenQuestions()
        {
            return db.Questions.Where(q => q.IsHidden == true);
        }

        // GET: api/Questions/5
        [HttpGet]
        [Route("api/questions/{id}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult GetQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpPut]
        [Route("api/questions/{id}")]
        public IHttpActionResult ChangeQuestionState(int Id)
        {
            Question question = db.Questions.FirstOrDefault(q => q.Id == Id);

            if(question == null)
            {
                return NotFound();
            }
            else
            {
                if (question.IsHidden)
                {
                    question.IsHidden = false;
                }
                else
                {
                    question.IsHidden = true;
                }

                db.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        // POST: api/Questions
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question)
        {
            question.Date = DateTime.Now;

            question.IsHidden = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(question);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [HttpDelete]
        [Route("api/questions/{id}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}