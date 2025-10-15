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
    public class CandidatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Candidates
     
        public ActionResult Index()
        {
            var candidates = db.Candidates.Include(c => c.JobPosition);
            return View(candidates.ToList());
        }

        // GET: Candidates/Details/5
        [Authorize]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates
                .Include(c => c.JobPosition)
                .FirstOrDefault(c => c.Id == id); if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // GET: Candidates/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.JobPositionId = new SelectList(db.JobPositions, "Id", "Title");
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,CVPath,Status,JobPositionId")] Candidate candidate, HttpPostedFileBase CVFile)
        {
            if (CVFile == null || CVFile.ContentLength == 0)
            {
                ModelState.AddModelError("CVPath", "Please upload your CV.");
            }
            var job = db.JobPositions.Include("Candidates").FirstOrDefault(j => j.Id == candidate.JobPositionId);
            if (job == null)
            {
                return HttpNotFound();
            }
            int acceptedCount = job.Candidates.Count(c => c.Status == Candidate.CandidateStatus.Accepted);
            if (acceptedCount >= job.MaxPositionsOpen)
            {
                ModelState.AddModelError("", "This job position is no longer accepting new candidates.");
                ViewBag.JobPositionId = new SelectList(db.JobPositions, "Id", "Title", candidate.JobPositionId);
                return View(candidate);
            }

            if (ModelState.IsValid)
            {
                var fileName = System.IO.Path.GetFileName(CVFile.FileName);
                var path = Server.MapPath("~/Uploads/CVs");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                var fullPath = System.IO.Path.Combine(path, fileName);
                CVFile.SaveAs(fullPath);

                candidate.CVPath = "/Uploads/CVs/" + fileName;

                db.Candidates.Add(candidate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobPositionId = new SelectList(db.JobPositions, "Id", "Title", candidate.JobPositionId);
            return View(candidate);
        }

        // GET: Candidates/Edit/5
       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Include(c => c.JobPosition).FirstOrDefault(c => c.Id == id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobPositionId = new SelectList(db.JobPositions, "Id", "Title", candidate.JobPositionId);
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,CVPath,Status,JobPositionId")] Candidate candidate, HttpPostedFileBase CVFile)
        {
            if (ModelState.IsValid)
            {
                var existingCandidate = db.Candidates
                    .Include(c => c.JobPosition.Candidates)
                    .FirstOrDefault(c => c.Id == candidate.Id);

                if (existingCandidate == null)
                {
                    return HttpNotFound();
                }
                int acceptedCount = existingCandidate.JobPosition.Candidates.Count(c => c.Status == Candidate.CandidateStatus.Accepted);

                bool isChangingToAccepted = existingCandidate.Status != Candidate.CandidateStatus.Accepted &&
                                            candidate.Status == Candidate.CandidateStatus.Accepted;

                if (isChangingToAccepted && acceptedCount >= existingCandidate.JobPosition.MaxPositionsOpen)
                {
                    ModelState.AddModelError("", "No more candidates can be accepted for this job position.");
                    candidate.JobPosition = db.JobPositions.Find(candidate.JobPositionId);
                    ViewBag.JobPositionId = new SelectList(db.JobPositions, "Id", "Title", candidate.JobPositionId);
                    return View(candidate);
                }

                if (CVFile != null && CVFile.ContentLength > 0)
                {
                    var fileName = System.IO.Path.GetFileName(CVFile.FileName);
                    var path = Server.MapPath("~/Uploads/CVs");
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    var fullPath = System.IO.Path.Combine(path, fileName);
                    CVFile.SaveAs(fullPath);

                    existingCandidate.CVPath = "/Uploads/CVs/" + fileName;
                }

                existingCandidate.Name = candidate.Name;
                existingCandidate.Email = candidate.Email;
                existingCandidate.Status = candidate.Status;

                var job = existingCandidate.JobPosition;

                if (acceptedCount >= job.MaxPositionsOpen)
                {
                    job.Status = "Closed";
                }
                else
                {
                    job.Status = "Open";
                }


                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.JobPositionId = new SelectList(db.JobPositions, "Id", "Title", candidate.JobPositionId);
            return View(candidate);
        }


        // GET: Candidates/Delete/5
        public ActionResult Delete(int id)
        {
            Candidate candidate = db.Candidates.Find(id);
            db.Candidates.Remove(candidate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Candidates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Candidate candidate = db.Candidates.Find(id);
        //    db.Candidates.Remove(candidate);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
