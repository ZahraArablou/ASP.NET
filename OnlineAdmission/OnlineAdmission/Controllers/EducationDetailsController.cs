using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAdmission.Models;
using OnlineAdmission.Helpers;

namespace OnlineAdmission.Controllers
{
    public class EducationDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EducationDetails
        public ActionResult Index()
        {
            string id = UserHelper.GetUserId(db.Users, User.Identity);
            var educationDetails = db.EducationDetails.Where(p=>p.ApplicationId==id).Include(e => e.Application);
            ViewBag.flag = IsCompleted();
            return View(educationDetails.ToList());
        }
        public bool IsCompleted()
        {
            string id = UserHelper.GetUserId(db.Users, User.Identity);
            var application = db.Applications.Where(p => p.ApplicationId == id).Include(a => a.Department).Include(a => a.Program).Include(a => a.User).SingleOrDefault();
            if (application != null)
            {
                if (application.Status == Status.Complete)
                    return false;
            }
            return true;
        }

        // GET: EducationDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDetail educationDetail = db.EducationDetails.Find(id);
            if (educationDetail == null)
            {
                return HttpNotFound();
            }
            return View(educationDetail);
        }
        // GET: EducationDetails/EducationAdmin/5
        public ActionResult EducationAdmin(string id)
        {
            var educationDetails = db.EducationDetails.Where(p => p.ApplicationId == id).Include(e => e.Application);
            ViewBag.applicationId = id;
            return View(educationDetails.ToList());
        }


        // GET: EducationDetails/Create
        public ActionResult Create()
        {
            //ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "Id");
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id");
            return View();
        }

        // POST: EducationDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationId,Qualification,Year,Duration,BoardUniversity,Subjects,Percentage")] EducationDetail educationDetail)
        {
            if (ModelState.IsValid)
            {
                educationDetail.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);

                db.EducationDetails.Add(educationDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "FirstName", educationDetail.ApplicationId);
            //ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id");
            return View(educationDetail);
        }

        // GET: EducationDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDetail educationDetail = db.EducationDetails.Find(id);
            if (educationDetail == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "FirstName", educationDetail.ApplicationId);
            ViewBag.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);
            return View(educationDetail);
        }

        // POST: EducationDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationId,Qualification,Year,Duration,BoardUniversity,Subjects,Percentage")] EducationDetail educationDetail)
        {
            if (ModelState.IsValid)
            {
                educationDetail.ApplicationId=UserHelper.GetUserId(db.Users, User.Identity); ;
                db.Entry(educationDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "FirstName", educationDetail.ApplicationId);
            return View(educationDetail);
        }

        // GET: EducationDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDetail educationDetail = db.EducationDetails.Find(id);
            if (educationDetail == null)
            {
                return HttpNotFound();
            }
            return View(educationDetail);
        }

        // POST: EducationDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EducationDetail educationDetail = db.EducationDetails.Find(id);
            db.EducationDetails.Remove(educationDetail);
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
