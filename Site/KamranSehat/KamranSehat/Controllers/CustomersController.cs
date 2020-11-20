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
using ViewModels;
using Helpers;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomersController : Controller
    {
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        private DatabaseContext db = new DatabaseContext();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.CustomerGroup).Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreationDate);
            return View(customers.ToList());
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CustomerGroupId = new SelectList(db.CustomerGroups, "Id", "Title");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Customer/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    customer.ImageUrl = newFilenameUrl;
                }
                #endregion
                customer.IsDeleted = false;
                customer.CreationDate = DateTime.Now;
                customer.Id = Guid.NewGuid();
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerGroupId = new SelectList(db.CustomerGroups, "Id", "Title", customer.CustomerGroupId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerGroupId = new SelectList(db.CustomerGroups, "Id", "Title", customer.CustomerGroupId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Customer/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    customer.ImageUrl = newFilenameUrl;
                }
                #endregion
                customer.IsDeleted = false;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerGroupId = new SelectList(db.CustomerGroups, "Id", "Title", customer.CustomerGroupId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Customer customer = db.Customers.Find(id);
            customer.IsDeleted = true;
            customer.DeletionDate = DateTime.Now;

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
        [Route("customer")]
        public ActionResult List()
        {
            CustomerListViewModel customer = new CustomerListViewModel();

            customer.MenuServiceGroups = baseViewModel.GetMenuServices();
            customer.MenuProductGroups = baseViewModel.GetMenu();
            customer.CustomerGroups= db.CustomerGroups.Where(current => current.IsDeleted == false && current.IsActive == true).OrderByDescending(current=>current.Order).ToList();
            customer.Customers = db.Customers.Where(current => current.IsDeleted == false && current.IsActive == true).OrderByDescending(current => current.Order).ToList();
            customer.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();

            //List<CustomerWithGroup> customerWithGroup = new List<CustomerWithGroup>();

            //List<CustomerGroup> customerGroups = db.CustomerGroups.Where(current => current.IsDeleted == false && current.IsActive == true).ToList();

            //foreach (CustomerGroup group in customerGroups)
            //{
            //    customerWithGroup.Add(new CustomerWithGroup
            //    {
            //        CustomerGroup=group,
            //        Customers=db.Customers.Where(current=>current.IsDeleted==false && current.IsActive==true && current.CustomerGroupId==group.Id).ToList()
            //    });
            //}

            //customer.MenuServiceGroups = baseViewModel.GetMenuServices();
            //customer.MenuProductGroups = baseViewModel.GetMenu();
            //customer.CustomerList = customerWithGroup;

            return View(customer);
        }
    }
}
