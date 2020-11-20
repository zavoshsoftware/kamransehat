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
    public class ProductGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductGroups
        public ActionResult Index()
        {
            var productGroups = db.ProductGroups.Include(p => p.Parent).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate);
            return View(productGroups.ToList());
        }

        // GET: ProductGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        // GET: ProductGroups/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.ProductGroups, "Id", "Title");
            return View();
        }

        // POST: ProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroup productGroup, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/ProductGroup/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    productGroup.ImageUrl = newFilenameUrl;
                }
#endregion
                productGroup.IsDeleted=false;
				productGroup.CreationDate= DateTime.Now; 
                productGroup.Id = Guid.NewGuid();
                db.ProductGroups.Add(productGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.ProductGroups, "Id", "Title", productGroup.ParentId);
            return View(productGroup);
        }

        // GET: ProductGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.ProductGroups, "Id", "Title", productGroup.ParentId);
            return View(productGroup);
        }

        // POST: ProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroup productGroup, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/ProductGroup/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    productGroup.ImageUrl = newFilenameUrl;
                }
                #endregion

                productGroup.IsDeleted=false;
                db.Entry(productGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.ProductGroups, "Id", "Title", productGroup.ParentId);
            return View(productGroup);
        }

        // GET: ProductGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        // POST: ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductGroup productGroup = db.ProductGroups.Find(id);
			productGroup.IsDeleted=true;
			productGroup.DeletionDate=DateTime.Now;
 
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
