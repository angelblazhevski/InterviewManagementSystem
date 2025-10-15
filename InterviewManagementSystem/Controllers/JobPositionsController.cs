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
    public class JobPositionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: JobPositions
        public ActionResult Index()
        {
            var jobPositions = db.JobPositions.Include(j => j.Candidates).ToList();
            foreach (var job in jobPositions)
            {
                int acceptedCount = job.Candidates.Count(c => c.Status == Candidate.CandidateStatus.Accepted);
                if (acceptedCount >= job.MaxPositionsOpen)
                {
                    if (job.Status != "Closed")
                    {
                        job.Status = "Closed";
                        db.Entry(job).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (job.Status != "Open")
                    {
                        job.Status = "Open";
                        db.Entry(job).State = EntityState.Modified;
                    }
                }
            }

            db.SaveChanges();
            return View(jobPositions);
        }

        // GET: JobPositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPosition jobPosition = db.JobPositions.Find(id);
            if (jobPosition == null)
            {
                return HttpNotFound();
            }
            return View(jobPosition);
        }

        // GET: JobPositions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobPositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Requirements,Status,MaxPositionsOpen")] JobPosition jobPosition)
        {
            if (ModelState.IsValid)
            {
                db.JobPositions.Add(jobPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobPosition);
        }

        // GET: JobPositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPosition jobPosition = db.JobPositions.Find(id);
            if (jobPosition == null)
            {
                return HttpNotFound();
            }
            return View(jobPosition);
        }

        // POST: JobPositions/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Requirements,Status,MaxPositionsOpen")] JobPosition jobPosition)
        {
            if (ModelState.IsValid)
            {
                var existingJob = db.JobPositions.Include(j => j.Candidates)
                                        .FirstOrDefault(j => j.Id == jobPosition.Id);

                if (existingJob == null)
                {
                    return HttpNotFound();
                }

                // Count accepted candidates
                int acceptedCount = existingJob.Candidates.Count(c => c.Status == Candidate.CandidateStatus.Accepted);

                // Validation: new MaxPositionsOpen cannot be less than accepted candidates
                if (jobPosition.MaxPositionsOpen < acceptedCount)
                {
                    ModelState.AddModelError("MaxPositionsOpen", $"Cannot set MaxPositionsOpen to {jobPosition.MaxPositionsOpen} because there are already {acceptedCount} accepted candidate(s).");
                    return View(jobPosition);
                }

                // Update allowed
                existingJob.Title = jobPosition.Title;
                existingJob.Requirements = jobPosition.Requirements;
                existingJob.MaxPositionsOpen = jobPosition.MaxPositionsOpen;
                existingJob.Status = acceptedCount >= jobPosition.MaxPositionsOpen ? "Closed" : "Open";

             //   db.Entry(jobPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobPosition);
        }

        // GET: JobPositions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobPosition jobPosition = db.JobPositions.Find(id);
        //    if (jobPosition == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobPosition);
        //}

        // POST: JobPositions/Delete/5
     //   [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            JobPosition jobPosition = db.JobPositions.Find(id);
            db.JobPositions.Remove(jobPosition);
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
