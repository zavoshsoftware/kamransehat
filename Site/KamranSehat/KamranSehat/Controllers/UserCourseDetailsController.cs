using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace KamranSehat.Controllers
{
    public class UserCourseDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: UserCourseDetails
        public ActionResult Index()
        {
            var userCourseDetails = db.UserCourseDetails.Include(u => u.CourseDetail).Where(u=>u.IsDeleted==false).OrderByDescending(u=>u.CreationDate).Include(u => u.User).Where(u=>u.IsDeleted==false).OrderByDescending(u=>u.CreationDate);
            return View(userCourseDetails.ToList());
        }

        // GET: UserCourseDetails/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCourseDetail userCourseDetail = db.UserCourseDetails.Find(id);
            if (userCourseDetail == null)
            {
                return HttpNotFound();
            }
            return View(userCourseDetail);
        }

        // GET: UserCourseDetails/Create
        public ActionResult Create()
        {
            ViewBag.CourseDetailId = new SelectList(db.CourseDetails, "Id", "Teachers");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: UserCourseDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,CourseDetailId,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] UserCourseDetail userCourseDetail)
        {
            if (ModelState.IsValid)
            {
				userCourseDetail.IsDeleted=false;
				userCourseDetail.CreationDate= DateTime.Now; 
                userCourseDetail.Id = Guid.NewGuid();
                db.UserCourseDetails.Add(userCourseDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseDetailId = new SelectList(db.CourseDetails, "Id", "Teachers", userCourseDetail.CourseDetailId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userCourseDetail.UserId);
            return View(userCourseDetail);
        }

        // GET: UserCourseDetails/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCourseDetail userCourseDetail = db.UserCourseDetails.Find(id);
            if (userCourseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseDetailId = new SelectList(db.CourseDetails, "Id", "Teachers", userCourseDetail.CourseDetailId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userCourseDetail.UserId);
            return View(userCourseDetail);
        }

        // POST: UserCourseDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,CourseDetailId,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] UserCourseDetail userCourseDetail)
        {
            if (ModelState.IsValid)
            {
				userCourseDetail.IsDeleted=false;
                db.Entry(userCourseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseDetailId = new SelectList(db.CourseDetails, "Id", "Teachers", userCourseDetail.CourseDetailId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userCourseDetail.UserId);
            return View(userCourseDetail);
        }

        // GET: UserCourseDetails/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCourseDetail userCourseDetail = db.UserCourseDetails.Find(id);
            if (userCourseDetail == null)
            {
                return HttpNotFound();
            }
            return View(userCourseDetail);
        }

        // POST: UserCourseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserCourseDetail userCourseDetail = db.UserCourseDetails.Find(id);
			userCourseDetail.IsDeleted=true;
			userCourseDetail.DeletionDate=DateTime.Now;
 
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
