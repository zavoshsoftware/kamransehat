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
    public class UsersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var users = db.Users.Where(u=>u.IsDeleted==false).OrderByDescending(u=>u.Code).Include(u => u.Role);
            return View(users.ToList());
        }

     
        public ActionResult Create()
        {
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Title");
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Password,CellNum,FullName,Code,BirthDate,Address,PostalCode,AvatarImageUrl,Email,GenderId,RoleId,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] User user)
        {
            if (ModelState.IsValid)
            {
				user.IsDeleted=false;
				user.CreationDate= DateTime.Now; 
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Title", user.GenderId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Title", user.GenderId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Password,CellNum,FullName,Code,BirthDate,Address,PostalCode,AvatarImageUrl,Email,GenderId,RoleId,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] User user)
        {
            if (ModelState.IsValid)
            {
				user.IsDeleted=false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Title", user.GenderId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(id);
			user.IsDeleted=true;
			user.DeletionDate=DateTime.Now;
 
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
