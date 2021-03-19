using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAdmission.Helpers;
using OnlineAdmission.Models;
using OnlineAdmission.ModelView;
using PagedList;

namespace OnlineAdmission.Controllers
{
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        Boolean flag = true;
        // GET: Applications
        public ActionResult Index()
        {
            string id = UserHelper.GetUserId(db.Users, User.Identity); 
            var application = db.Applications.Where(p=>p.ApplicationId==id).Include(a => a.Department).Include(a => a.Program).Include(a => a.User).SingleOrDefault() ;
            ViewBag.flag =IsCompleted();
            return View(application);
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
        public ActionResult IndexAdmin(string sortDir, string searchString, string currentFilter, string searchStatus, string currentStatus, int? page, string sortOrder = "")
        {

            //ViewBag.SearchString = searchString;
            //var applications = db.Applications.Include(a => a.Department).Include(a => a.Program).Include(a => a.User).OrderBy(h => h.FirstName).AsQueryable();

            //if (!string.IsNullOrEmpty(searchString))
            //    applications = applications.Where(s => s.FirstName.Contains(searchString));
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortDir = sortDir;
            sortOrder = sortOrder + "_" + sortDir;
            bool flag = false;
            if (searchStatus !="")
            {
                //page = 1;
                ViewBag.currentStatus = searchStatus;
                flag = true;
            }

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var applications = db.Applications.Include(h => h.Department).Include(h => h.Program).Include(a => a.User);

            if (!String.IsNullOrEmpty(searchString)&& !flag)
                applications = applications.Where(p => p.FirstName.Contains(searchString));
            else
            if (!String.IsNullOrEmpty(searchStatus) && flag)
                applications = applications.Where(p => p.StatusString.Contains(searchStatus));

            switch (sortOrder.ToLower())
            {
                case "firstname_desc":
                    applications = applications.OrderByDescending(p => p.FirstName);
                    break;
                case "department_asc":
                    applications = applications.OrderBy(h => h.Department.Name);
                    break;
                case "department_desc":
                    applications = applications.OrderByDescending(p => p.Department.Name);
                    break;
                case "program_asc":
                    applications = applications.OrderBy(p => p.Program.Name);
                    break;
                case "program_desc":
                    applications = applications.OrderByDescending(p => p.Program.Name);
                    break;
                case "registrationdate_asc":
                    applications = applications.OrderBy(p => p.RegistrationDate);
                    break;
                case "registrationdate_desc":
                    applications = applications.OrderByDescending(p => p.RegistrationDate);
                    break;
                case "status_desc":
                    applications = applications.OrderByDescending(p => p.StatusString);
                    break;
                default:  // Description ascending 
                    applications = applications.OrderBy(p => p.FirstName);
                    break;
            }

            int pageSize =2;
            int pageNumber = (page ?? 1);
            var data = applications.ToPagedList(pageNumber, pageSize);

            return View("IndexAdmin", data);

        }


        // GET: Applications/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Application application = db.Applications.Where(p => p.ApplicationId == id).Include(a => a.Department).Include(a => a.Program).Include(a => a.User).FirstOrDefault();
            //Application application = db.Applications.Find(id);

            ViewBag.ApplicationId = id;
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }


        public ActionResult DetailsAdmin(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Where(p => p.ApplicationId == id).Include(a => a.Department).Include(a => a.Program).Include(a => a.User).FirstOrDefault();
            //Application application = db.Applications.Find(id);

            ViewBag.ApplicationId = id;
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }




        // GET: Applications/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.ProgarmId = new SelectList(db.Programs, "ProgarmId", "Name");
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ApplicationId,FirstName,LastName,TelePhoneNumber,DepartmentId,ProgarmId,RegistrationDate,StatusString")] Application application)
        public ActionResult Create(ApplicationViewModel applicationVM)
        {
            Application application = new Application();

            if (ModelState.IsValid)
            {
                application.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);
                application.FirstName = applicationVM.FirstName;
                application.LastName = applicationVM.LastName;
                application.ProgarmId = applicationVM.ProgarmId;
                application.DepartmentId = applicationVM.DepartmentId;
                application.TelePhoneNumber = applicationVM.TelePhoneNumber;
                application.Status = Status.Draft;
                application.RegistrationDate = DateTime.Now;
                if (applicationVM.Photo != null)
                    application.Photo = ImageConverter.ByteArrayFromPostedFile(applicationVM.Photo);

                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", applicationVM.DepartmentId);
            ViewBag.ProgarmId = new SelectList(db.Programs, "ProgarmId", "Name", applicationVM.ProgarmId);
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id", applicationVM.ApplicationId);
            return View(applicationVM);
        
    }

        // GET: Applications/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ApplicationViewModel applicationViewModel = new ApplicationViewModel();

            applicationViewModel.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);
            applicationViewModel.FirstName = application.FirstName;
            applicationViewModel.LastName = application.LastName;
            applicationViewModel.ProgarmId = application.ProgarmId;
            applicationViewModel.DepartmentId = application.DepartmentId;
            applicationViewModel.TelePhoneNumber = application.TelePhoneNumber;
            //applicationViewModel.Status = Status.Saved;
            application.RegistrationDate = DateTime.Now;
            if (application.Photo != null)
                applicationViewModel.PhotoDB = application.Photo;



            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", applicationViewModel.DepartmentId);
            ViewBag.ProgarmId = new SelectList(db.Programs, "ProgarmId", "Name", applicationViewModel.ProgarmId);
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id", applicationViewModel.ApplicationId);
            return View(applicationViewModel);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationViewModel applicationVM)
        {
            //Application application = new Application();

            if (ModelState.IsValid)
            {
                //application.ApplicationId = UserHelper.GetUserId(db.Users, User.Identity);
                Application application = db.Applications.Find(applicationVM.ApplicationId);
                if (applicationVM != null && applicationVM.Photo != null)
                    application.Photo = ImageConverter.ByteArrayFromPostedFile(applicationVM.Photo);
                application.FirstName = applicationVM.FirstName;
                application.LastName = applicationVM.LastName;
                application.ProgarmId = applicationVM.ProgarmId;
                application.DepartmentId = applicationVM.DepartmentId;
                application.TelePhoneNumber = applicationVM.TelePhoneNumber;
                application.Status = Status.Draft;
                application.RegistrationDate = DateTime.Now;

                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", applicationVM.DepartmentId);
            ViewBag.ProgarmId = new SelectList(db.Programs, "ProgarmId", "Name", applicationVM.ProgarmId);
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Id", applicationVM.ApplicationId);
            return View(applicationVM);
        }
        
        public ActionResult FinishApplication()
        {
            return View("_ConfirmStatus");

        }
        public ActionResult SubmitApplication()
        {
            Application application = db.Applications.Find(UserHelper.GetUserId(db.Users, User.Identity));
            if (application == null)
            {
                return HttpNotFound();
            }
            application.Status = Status.Complete;
            db.Entry(application).State = EntityState.Modified;
            db.SaveChanges();
            flag = false;
            return View("_statusChanged");
        }
        // GET: Applications/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
