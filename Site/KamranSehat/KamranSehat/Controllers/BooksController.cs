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
    public class BooksController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }
        
        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book, HttpPostedFileBase fileUpload, HttpPostedFileBase bookUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Book/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    book.ImageUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize audio if needed
                if (bookUpload != null)
                {
                    string bookname = Path.GetFileName(bookUpload.FileName);
                    string newBookname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(bookname);

                    string newBooknameUrl = "/Uploads/Book/" + newBookname;
                    string physicalBookname = Server.MapPath(newBooknameUrl);

                    bookUpload.SaveAs(physicalBookname);

                    book.BookUrl = newBooknameUrl;
                }
                #endregion
                book.IsDeleted=false;
				book.CreationDate= DateTime.Now; 
                book.Id = Guid.NewGuid();
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, HttpPostedFileBase fileUpload, HttpPostedFileBase bookUpload)
        {
            if (ModelState.IsValid)
            {
                string newFilenameUrl = book.ImageUrl;
                string newBooknameUrl = book.BookUrl;
                #region Upload and resize image if needed
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                     newFilenameUrl = "/Uploads/Book/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    book.ImageUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize audio if needed
                if (bookUpload != null)
                {
                    string bookname = Path.GetFileName(bookUpload.FileName);
                    string newBookname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(bookname);

                     newBooknameUrl = "/Uploads/Book/" + newBookname;
                    string physicalBookname = Server.MapPath(newBooknameUrl);

                    bookUpload.SaveAs(physicalBookname);

                    book.BookUrl = newBooknameUrl;
                }
                #endregion
                book.IsDeleted=false;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Book book = db.Books.Find(id);
			book.IsDeleted=true;
			book.DeletionDate=DateTime.Now;
 
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
        [Route("book")]
        public ActionResult List()
        {

            BookListViewModel book = new BookListViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),

                Books =
                    db.Books.Where(current => current.IsDeleted == false && current.IsActive)
                        .OrderByDescending(current => current.CreationDate).ToList()
            };
            ViewBag.Canonical = "https://kamransehat.ir/book/";
            return View(book);
        }

        [AllowAnonymous]
        [Route("book/{code}")]
        public ActionResult Details(string code)
        {
            ViewBag.Canonical = "https://kamransehat.ir/book/" + code;

            Book book = db.Books.FirstOrDefault(current => current.Code == code && current.IsDeleted == false);

            //invalid product code
            if (book == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("notfound", "Home");
            }

            //Invalid Product Group


            BookDetailViewModel bookDetail = new BookDetailViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),

                Book = book,
            };
            return View(bookDetail);
        }
    }
}
