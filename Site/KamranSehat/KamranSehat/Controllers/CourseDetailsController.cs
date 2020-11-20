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
    [Authorize(Roles = "Administrator")]
    public class CourseDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var courseDetails = db.CourseDetails.Include(c => c.Course).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(courseDetails.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title");
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PresentDate,Teachers,CourseId,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] CourseDetail courseDetail)
        {
            if (ModelState.IsValid)
            {
				courseDetail.IsDeleted=false;
				courseDetail.CreationDate= DateTime.Now; 
                courseDetail.Id = Guid.NewGuid();
                db.CourseDetails.Add(courseDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseDetail.CourseId);
            return View(courseDetail);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseDetail.CourseId);
            return View(courseDetail);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PresentDate,Teachers,CourseId,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] CourseDetail courseDetail)
        {
            if (ModelState.IsValid)
            {
				courseDetail.IsDeleted=false;
                db.Entry(courseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseDetail.CourseId);
            return View(courseDetail);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            return View(courseDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CourseDetail courseDetail = db.CourseDetails.Find(id);
			courseDetail.IsDeleted=true;
			courseDetail.DeletionDate=DateTime.Now;
 
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
