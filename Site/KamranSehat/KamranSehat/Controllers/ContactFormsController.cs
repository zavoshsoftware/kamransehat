using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Models;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ContactFormsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.ContactForms.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Message,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
				contactForm.IsDeleted=false;
				contactForm.CreationDate= DateTime.Now; 
                contactForm.Id = Guid.NewGuid();
                db.ContactForms.Add(contactForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactForm);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = db.ContactForms.Find(id);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Message,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
				contactForm.IsDeleted=false;
                db.Entry(contactForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactForm);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = db.ContactForms.Find(id);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ContactForm contactForm = db.ContactForms.Find(id);
			contactForm.IsDeleted=true;
			contactForm.DeletionDate=DateTime.Now;
 
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostContactForm(string Name, string Email, string Message)
        {
            bool isEmail = Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (isEmail)
            {
                ContactForm cf = new ContactForm();
                cf.Id = Guid.NewGuid();
                cf.Email = Email;
                cf.IsDeleted = false;
                cf.Message = Message;
                cf.Name = Name;
                cf.CreationDate = DateTime.Now;

                db.ContactForms.Add(cf);
                db.SaveChanges();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("false", JsonRequestBehavior.AllowGet);

        }
    }
}
