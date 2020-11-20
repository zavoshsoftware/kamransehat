using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;
using Helpers;
using System.IO;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RadiosController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();

        // GET: Radios
        public ActionResult Index()
        {
            return View(db.Radios.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

       

        // GET: Radios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Radios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Radio radio, HttpPostedFileBase fileUpload, HttpPostedFileBase radioUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Radio/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    radio.ImageUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize audio if needed
                if (radioUpload != null)
                {
                    string radioname = Path.GetFileName(radioUpload.FileName);
                    string newRadioname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(radioname);

                    string newRadionameUrl = "/Uploads/Radio/" + newRadioname;
                    string physicalRadioname = Server.MapPath(newRadionameUrl);

                    radioUpload.SaveAs(physicalRadioname);

                    radio.RedioUrl = newRadionameUrl;
                }
                #endregion
                radio.IsDeleted = false;
                radio.CreationDate = DateTime.Now;
                radio.Id = Guid.NewGuid();
                db.Radios.Add(radio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(radio);
        }

        // GET: Radios/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Radio radio = db.Radios.Find(id);
            if (radio == null)
            {
                return HttpNotFound();
            }
            return View(radio);
        }

        // POST: Radios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Radio radio, HttpPostedFileBase fileUpload, HttpPostedFileBase radioUpload)
        {
            if (ModelState.IsValid)
            {
                string newFilenameUrl = radio.ImageUrl;
                string newRadionameUrl = radio.RedioUrl;
                #region Upload and resize image if needed
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/Radio/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    radio.ImageUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize audio if needed
                if (radioUpload != null)
                {
                    string radioname = Path.GetFileName(radioUpload.FileName);
                    string newRadioname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(radioname);

                    newRadionameUrl = "/Uploads/Radio/" + newRadioname;
                    string physicalRadioname = Server.MapPath(newRadionameUrl);

                    radioUpload.SaveAs(physicalRadioname);

                    radio.RedioUrl = newRadionameUrl;
                }
                #endregion
                radio.IsDeleted = false;
                db.Entry(radio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(radio);
        }

        // GET: Radios/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Radio radio = db.Radios.Find(id);
            if (radio == null)
            {
                return HttpNotFound();
            }
            return View(radio);
        }

        // POST: Radios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Radio radio = db.Radios.Find(id);
            radio.IsDeleted = true;
            radio.DeletionDate = DateTime.Now;

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

        [AllowAnonymous]
        [Route("Radio")]
        public ActionResult List()
        {


            RadioListViewModel radio = new RadioListViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                RadioList =
                    db.Radios.Where(current => current.IsDeleted == false && current.IsActive)
                        .OrderByDescending(current => current.CreationDate).ToList()
            };
            ViewBag.Canonical = "https://kamransehat.ir/radio/";
            return View(radio);
        }

        [AllowAnonymous]
        [Route("Radio/{code}")]
        public ActionResult Details(string code)
        {
            ViewBag.Canonical = "https://kamransehat.ir/radio/" + code ;

            Radio radio = db.Radios.FirstOrDefault(current => current.Code == code && current.IsDeleted == false);

            //invalid product code
            if (radio == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("notfound", "Home");
            }

            //Invalid Product Group


            RadioDetailViewModel radioDetail = new RadioDetailViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),

                Radio = radio,
            };
            return View(radioDetail);
        }
    }
}
