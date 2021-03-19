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
using OnlineAdmission.ModelView;
using System.IO;
using System.Data.Entity.Migrations;

namespace OnlineAdmission.Controllers
{
    public class EnclosedDocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EnclosedDocuments
        public ActionResult Index()
        {
            string id = UserHelper.GetUserId(db.Users, User.Identity);
          

            var enclosedDocuments = db.EnclosedDocuments.Where(p => p.ApplicationId == id).Include(e => e.Application);
            ViewBag.flag = IsCompleted();
            return View(enclosedDocuments.ToList());
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
        // GET: EnclosedDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnclosedDocument enclosedDocument = db.EnclosedDocuments.Find(id);
            if (enclosedDocument == null)
            {
                return HttpNotFound();
            }
            return View(enclosedDocument);
        }

       public ActionResult DownLoadEnclosedDocuments(int id)
        {
            var enclosedDocument = db.EnclosedDocuments.Find(id);
            if (enclosedDocument != null)
                return File(enclosedDocument.DocumentFile, "application/octet-stream", enclosedDocument.Name+ ".pdf");
            return null;
        }

        public ActionResult EnclosedDocumentsAdmin(string id)
        {
            var enclosedDocuments = db.EnclosedDocuments.Where(p => p.ApplicationId == id).Include(e => e.Application);
            ViewBag.applicationId = id;
            return View(enclosedDocuments.ToList());

        }
        
        // GET: EnclosedDocuments/Create
        public ActionResult Create()
        {
            //ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "FirstName");
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id");
            return View();
        }

        // POST: EnclosedDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ApplicationId,DocumentType,Name,DocumentFile")] EnclosedDocument enclosedDocument)
        public ActionResult Create( DocumentsViewModel documentsViewModel)
        {
            if (ModelState.IsValid)
            {
                EnclosedDocument enclosedDocument = new EnclosedDocument();
                enclosedDocument.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);
                enclosedDocument.DocumentType = documentsViewModel.DocumentType;
                enclosedDocument.Name = documentsViewModel.Name;
                 if (documentsViewModel.DocumentFile != null)
                    enclosedDocument.DocumentFile = ImageConverter.ByteArrayFromPostedFile(documentsViewModel.DocumentFile);
              
                db.EnclosedDocuments.Add(enclosedDocument);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "ApplicationId",documentsViewModel.ApplicationId);
            return View(documentsViewModel);
        }

        // GET: EnclosedDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnclosedDocument enclosedDocument = db.EnclosedDocuments.Find(id);
            DocumentsViewModel enclosedDocumentVM = new DocumentsViewModel();
            enclosedDocumentVM.ApplicationId = enclosedDocument.ApplicationId;
            enclosedDocumentVM.DocumentType = enclosedDocument.DocumentType;
            enclosedDocumentVM.Name = enclosedDocument.Name;
           if (enclosedDocument.DocumentFile != null)
                enclosedDocumentVM.DocumentFileDB = enclosedDocument.DocumentFile;

            if (enclosedDocumentVM == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "ApplicationId", enclosedDocumentVM.ApplicationId);
            return View(enclosedDocumentVM);
        }

        // POST: EnclosedDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentsViewModel documentsViewModel)
        {
            if (ModelState.IsValid)
            {
             
                EnclosedDocument enclosedDocument = db.EnclosedDocuments.Find(documentsViewModel.Id);
                enclosedDocument.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);
                enclosedDocument.DocumentType = documentsViewModel.DocumentType;
                enclosedDocument.Name = documentsViewModel.Name;
                if (documentsViewModel.DocumentFile != null)
                    enclosedDocument.DocumentFile = ImageConverter.ByteArrayFromPostedFile(documentsViewModel.DocumentFile);
                db.Entry(enclosedDocument).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationId = new SelectList(db.Applications, "ApplicationId", "ApplicationId", documentsViewModel.ApplicationId);
            return View(documentsViewModel);
        }

        // GET: EnclosedDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnclosedDocument enclosedDocument = db.EnclosedDocuments.Find(id);
            if (enclosedDocument == null)
            {
                return HttpNotFound();
            }
            return View(enclosedDocument);
        }

        // POST: EnclosedDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnclosedDocument enclosedDocument = db.EnclosedDocuments.Find(id);
            db.EnclosedDocuments.Remove(enclosedDocument);
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
