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

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ConsultationsController : Controller
    {
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        private DatabaseContext db = new DatabaseContext();

        // GET: Consultations
        public ActionResult Index()
        {
            return View(db.Consultations.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

        [Route("consult-request")]
        [AllowAnonymous]
        public ActionResult Details()
        {
            ConsultationDetailViewModel consultationDetail = new ConsultationDetailViewModel();
            consultationDetail.MenuProductGroups = baseViewModel.GetMenu();
            consultationDetail.MenuServiceGroups = baseViewModel.GetMenuServices();
            consultationDetail.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();

            TempData["success"] = "false";
            ReturnDropDowns(null,null,null);
            return View(consultationDetail);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(ConsultationDetailViewModel viewmodel)
        {
            ConsultationDetailViewModel consultationDetail = new ConsultationDetailViewModel();
            consultationDetail.MenuProductGroups = baseViewModel.GetMenu();
            consultationDetail.MenuServiceGroups = baseViewModel.GetMenuServices();

            if (ModelState.IsValid)
            {
                viewmodel.Consultation.IsDeleted = false;
                viewmodel.Consultation.CreationDate = DateTime.Now;
                viewmodel.Consultation.Id = Guid.NewGuid();
                db.Consultations.Add(viewmodel.Consultation);
                db.SaveChanges();
                TempData["success"] = "true";

                consultationDetail.Consultation = viewmodel.Consultation;
                ReturnDropDowns(null, null, null);
                return View(consultationDetail);
            }
            ReturnDropDowns(null, null, null);
            TempData["success"] = "false";
            return View(consultationDetail);
        }
        // GET: Consultations/Create
        public ActionResult Create()
        {
            ReturnDropDowns(null, null, null);
            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                consultation.IsDeleted = false;
                consultation.CreationDate = DateTime.Now;
                consultation.Id = Guid.NewGuid();
                db.Consultations.Add(consultation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ReturnDropDowns(null,null,null);
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            ReturnDropDowns(consultation.DayOfWeek, consultation.Time, consultation.Type);
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                consultation.IsDeleted = false;
                db.Entry(consultation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ReturnDropDowns(consultation.DayOfWeek, consultation.Time, consultation.Type);
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Consultation consultation = db.Consultations.Find(id);
            consultation.IsDeleted = true;
            consultation.DeletionDate = DateTime.Now;

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

        public void ReturnDropDowns(string selectedDay, string selectedTime, string selectedType)
        {
            List<SelectListItem> day = new List<SelectListItem>();
            day.Insert(0, (new SelectListItem { Text = "شنبه", Value = "شنبه" }));
            day.Insert(1, (new SelectListItem { Text = "یکشنبه", Value = "یکشنبه" }));
            day.Insert(2, (new SelectListItem { Text = "دوشنبه", Value = "دوشنبه" }));
            day.Insert(3, (new SelectListItem { Text = "سه شنبه", Value = "سه شنبه" }));
            day.Insert(4, (new SelectListItem { Text = "چهارشنبه", Value = "چهارشنبه" }));
            day.Insert(5, (new SelectListItem { Text = "پنح شنبه", Value = "پنح شنبه" }));


            if (!string.IsNullOrEmpty(selectedDay))
                ViewBag.Consultation_DayOfWeek = new SelectList(day, "Value", "Text", selectedDay);
            else
                ViewBag.Consultation_DayOfWeek = new SelectList(day, "Value", "Text");

            List<SelectListItem> time = new List<SelectListItem>();
            time.Insert(0, (new SelectListItem { Text = "8 تا 11", Value = "8 تا 11" }));
            time.Insert(1, (new SelectListItem { Text = "11 تا 14", Value = "11 تا 14" }));
            time.Insert(2, (new SelectListItem { Text = "14 تا 17", Value = "14 تا 17" }));
            time.Insert(3, (new SelectListItem { Text = "17 تا 20", Value = "17 تا 20" }));
            if (!string.IsNullOrEmpty(selectedTime))
                ViewBag.Consultation_Time = new SelectList(time, "Value", "Text",selectedTime);
            else
                ViewBag.Consultation_Time = new SelectList(time, "Value", "Text");


            List<SelectListItem> type = new List<SelectListItem>();
            type.Insert(0, (new SelectListItem { Text = "تلفنی", Value = "تلفنی" }));
            type.Insert(1, (new SelectListItem { Text = "حضوری", Value = "حضوری" }));
            type.Insert(2, (new SelectListItem { Text = "آنلاین", Value = "آنلاین" }));
            if (!string.IsNullOrEmpty(selectedType))
                ViewBag.Consultation_Type = new SelectList(type, "Value", "Text",selectedType);
            else
                ViewBag.Consultation_Type = new SelectList(type, "Value", "Text");
        }
    }
}
