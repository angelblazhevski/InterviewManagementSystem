using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InterviewManagementSystem.Models;

namespace InterviewManagementSystem.Controllers
{
    [Authorize]
    public class InterviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Interviews
        public ActionResult Index()
        {
            var interviews = db.Interviews.Include(i => i.Candidate).Include(i => i.Interviewer);
            return View(interviews.ToList());
        }

        // GET: Interviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var interview = db.Interviews.Include(i => i.Candidate).Include(i => i.Interviewer).FirstOrDefault(i => i.Id == id); if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // GET: Interviews/Create
        public ActionResult Create(int? interviewerId)
        {
            ViewBag.CandidateId = new SelectList(db.Candidates, "Id", "Name");
            var interviewerSelectList = new SelectList(db.Interviewers, "Id", "Name", interviewerId);
            ViewBag.InterviewerId = interviewerSelectList;
            ViewBag.InterviewerReadonly = interviewerId.HasValue;
            return View();
        }

        // POST: Interviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Notes,CandidateId,InterviewerId")] Interview interview)
        {
            bool alreadyExists = db.Interviews.Any(i => i.CandidateId == interview.CandidateId && i.InterviewerId == interview.InterviewerId);

            if (alreadyExists)
            {
                ModelState.AddModelError("", "This candidate has already been interviewed by the selected interviewer.");
            }
            if (ModelState.IsValid)
            {
                db.Interviews.Add(interview);
                db.SaveChanges();
                return RedirectToAction("Details", "Interviewers", new { id = interview.InterviewerId });
            }

            ViewBag.CandidateId = new SelectList(db.Candidates, "Id", "Name", interview.CandidateId);
            ViewBag.InterviewerId = new SelectList(db.Interviewers, "Id", "Name", interview.InterviewerId);
            return RedirectToAction("Details", "Interviewers", new { id = interview.InterviewerId });
        }

        // GET: Interviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            ViewBag.CandidateId = new SelectList(db.Candidates, "Id", "Name", interview.CandidateId);
            ViewBag.InterviewerId = new SelectList(db.Interviewers, "Id", "Name", interview.InterviewerId);
            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Notes,CandidateId,InterviewerId")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CandidateId = new SelectList(db.Candidates, "Id", "Name", interview.CandidateId);
            ViewBag.InterviewerId = new SelectList(db.Interviewers, "Id", "Name", interview.InterviewerId);
            return View(interview);
        }

        // GET: Interviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var interview = db.Interviews.Include(i => i.Candidate).Include(i => i.Interviewer).FirstOrDefault(i => i.Id == id); if (interview == null)
                if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interview interview = db.Interviews.Find(id);
            db.Interviews.Remove(interview);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
