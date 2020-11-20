using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BlogGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: BlogGroups
        public ActionResult Index()
        {
            return View(db.BlogGroups.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: BlogGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            if (blogGroup == null)
            {
                return HttpNotFound();
            }
            return View(blogGroup);
        }

        // GET: BlogGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogGroup blogGroup, HttpPostedFileBase fileupload)
        {


            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/BlogGroup/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    blogGroup.ImageUrl = newFilenameUrl;
                }
                #endregion
                blogGroup.IsDeleted = false;
                blogGroup.CreationDate = DateTime.Now;
                blogGroup.LastModifiedDate = DateTime.Now;
                blogGroup.Id = Guid.NewGuid();
                db.BlogGroups.Add(blogGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogGroup);
        }

        // GET: BlogGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            if (blogGroup == null)
            {
                return HttpNotFound();
            }
            return View(blogGroup);
        }

        // POST: BlogGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogGroup blogGroup, HttpPostedFileBase fileupload)
        {


            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/BlogGroup/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    blogGroup.ImageUrl = newFilenameUrl;
                }
                #endregion
                blogGroup.IsDeleted = false;
                blogGroup.LastModifiedDate = DateTime.Now;
                db.Entry(blogGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogGroup);
        }
        // GET: BlogGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogGroup blogGroup = db.BlogGroups.Find(id);
            if (blogGroup == null)
            {
                return HttpNotFound();
            }
            return View(blogGroup);
        }

        // POST: BlogGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BlogGroup blogGroup = db.BlogGroups.Find(id);
			blogGroup.IsDeleted=true;
			blogGroup.DeletionDate=DateTime.Now;
 
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
