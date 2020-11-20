using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using System.IO;
using ViewModels;
using Helpers;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ServicesController : Controller
    {
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        private DatabaseContext db = new DatabaseContext();

        // GET: Services
        public ActionResult Index()
        {
            var services = db.Services.Include(s => s.ServiceGroup).Where(s => s.IsDeleted == false).OrderByDescending(s => s.CreationDate);
            return View(services.ToList());
        }

        [AllowAnonymous]
        [Route("service/{servicegroupname}/{name}")]
        public ActionResult Details(string servicegroupname, string name)
        {
            ServiceDetailViewModel serviceDetail = new ServiceDetailViewModel();
            Service service = db.Services.Include(current=>current.ServiceGroup).FirstOrDefault(current => current.Name == name && current.IsDeleted == false && current.IsActive == true);

            if (service != null)
            {
                if (service.ServiceGroup.Name != servicegroupname)
                    return RedirectPermanent("/service" + service.ServiceGroup.Name + "/" + name);

                serviceDetail.MenuProductGroups = baseViewModel.GetMenu();
                serviceDetail.MenuServiceGroups = baseViewModel.GetMenuServices();
                serviceDetail.Service = service;
                serviceDetail.RelatedServices = db.Services.Where(current =>
                    current.ServiceGroupId == service.ServiceGroupId && current.Id != service.Id &&
                    current.IsDeleted == false && current.IsActive == true).ToList();
                serviceDetail.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();

                return View(serviceDetail);
            }

            return RedirectPermanent("/");
        }


        public ActionResult Create()
        {
            ViewBag.Duplicate = "false";
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups.Where(current => current.IsDeleted == false), "Id", "Title");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                bool duplicateService = db.Services.Where(current => current.IsDeleted == false && current.Name == service.Name).Any();
                if (!duplicateService)
                {
                    #region Upload and resize image if needed
                    if (fileupload != null)
                    {
                        string filename = Path.GetFileName(fileupload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        string newFilenameUrl = "/Uploads/Service/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileupload.SaveAs(physicalFilename);

                        service.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    service.IsDeleted = false;
                    service.CreationDate = DateTime.Now;
                    service.Id = Guid.NewGuid();
                    db.Services.Add(service);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    ViewBag.Duplicate = "true";
            }

            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups.Where(current => current.IsDeleted == false), "Id", "Title", service.ServiceGroupId);
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.Duplicate = "false";
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups, "Id", "Title", service.ServiceGroupId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                bool duplicateService = db.Services.Where(current => current.IsDeleted == false && current.Name == service.Name && current.Id != service.Id).Any();
                if (!duplicateService)
                {
                    #region Upload and resize image if needed
                    if (fileupload != null)
                    {
                        string filename = Path.GetFileName(fileupload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        string newFilenameUrl = "/Uploads/Service/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileupload.SaveAs(physicalFilename);

                        service.ImageUrl = newFilenameUrl;
                    }
                    #endregion
                    service.IsDeleted = false;
                    db.Entry(service).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    ViewBag.Duplicate = "true";
            }
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups.Where(current => current.IsDeleted == false), "Id", "Title", service.ServiceGroupId);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Service service = db.Services.Find(id);
            service.IsDeleted = true;
            service.DeletionDate = DateTime.Now;

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
