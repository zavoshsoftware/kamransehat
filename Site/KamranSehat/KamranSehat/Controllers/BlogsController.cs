using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BlogsController : Controller
    {
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        private DatabaseContext db = new DatabaseContext();

        // GET: Blogs
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.BlogGroup).Where(b => b.IsDeleted == false).OrderByDescending(b => b.CreationDate);
            return View(blogs.ToList());
        }



        // GET: Blogs/Create
        public ActionResult Create()
        {
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted = false;
                blog.CreationDate = DateTime.Now;
                blog.LastModifiedDate = DateTime.Now;
                blog.Id = Guid.NewGuid();
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title", blog.BlogGroupId);
            return View(blog);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title", blog.BlogGroupId);
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                blog.LastModifiedDate = DateTime.Now;
                blog.IsDeleted = false;
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title", blog.BlogGroupId);
            return View(blog);
        }
        // GET: Blogs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Blog blog = db.Blogs.Find(id);
            blog.IsDeleted = true;
            blog.DeletionDate = DateTime.Now;

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
        [Route("blog/{urlParam}")]
        public ActionResult List(string urlParam)
        {
            BlogListViewModel blogList = new BlogListViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
            };

            if (urlParam.ToLower() == "all")
            {
                blogList.Blogs = db.Blogs.Include(b => b.BlogGroup).Where(b => b.IsDeleted == false && b.IsActive)
                    .OrderByDescending(b => b.CreationDate).ToList();

                blogList.BlogGroupTitle = "مطالب وبلاگ";

                ViewBag.Title = "وبلاگ";
                ViewBag.Description =
                    "در این بخش از وب سایت رسمی دکتر کامران صحت می توانید جدیدترین مقالات در حوزه های مختلف را مطالعه نمایید.";

                blogList.BlogGroupSummery =
                    "در این بخش از وب سایت رسمی دکتر کامران صحت می توانید جدیدترین مقالات در حوزه های مختلف را مطالعه نمایید.";
            }
            else
            {


                BlogGroup blogGroup = db.BlogGroups.FirstOrDefault(current =>
                    current.UrlParam == urlParam && current.IsDeleted == false && current.IsActive);

                if (blogGroup != null)
                {
                    blogList.Blogs = db.Blogs.Include(b => b.BlogGroup)
                        .Where(b => b.IsDeleted == false && b.IsActive && b.BlogGroupId == blogGroup.Id)
                        .OrderByDescending(b => b.CreationDate).ToList();

                    ViewBag.Title = blogGroup.Title;
                    ViewBag.Description = blogGroup.Summery;

                    blogList.BlogGroupTitle = blogGroup.Title;
                    blogList.UrlParam = blogGroup.UrlParam;
                    blogList.BlogGroupSummery = blogGroup.Summery;
                    blogList.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();

                }
                else
                {
                    return RedirectToAction("List", new { urlParam = "all" });
                }
            }
            return View(blogList);
        }

        [AllowAnonymous]
        [Route("blog")]
        public ActionResult ListRedirect()
        {
            return RedirectPermanent("/blog/all");
        }

        [AllowAnonymous]
        [Route("blog/{groupUrlParam}/{urlParam}")]
        public ActionResult Details(string groupUrlParam, string urlParam)
        {
            Blog blog = db.Blogs.FirstOrDefault(current => current.UrlParam == urlParam && current.IsDeleted == false && current.IsActive);
            if (blog == null)
            {

                Response.StatusCode = 404;
                return RedirectToAction("AccessDenied", "Home");
            }

            if (blog.BlogGroup.UrlParam == groupUrlParam)
            {
                blog.Visit = blog.Visit + 1;
                db.SaveChanges();

                BlogDetailViewModel blogDetail = new BlogDetailViewModel()
                {
                    MenuProductGroups = baseViewModel.GetMenu(),
                    MenuServiceGroups = baseViewModel.GetMenuServices()
                };
                blogDetail.Blog = blog;

                blogDetail.BlogComments = db.BlogComments
                    .Where(current => current.BlogId == blog.Id && current.IsActive && !current.IsDeleted).ToList();

                blogDetail.BlogGroups = db.BlogGroups.Where(current => current.IsActive && !current.IsDeleted).ToList();

                blogDetail.RecentBlogs = db.Blogs
                    .Where(current => current.Id != blog.Id && current.IsActive && !current.IsDeleted)
                    .OrderByDescending(current => current.CreationDate).Take(6).ToList();

                blogDetail.RelatedBlogs = db.Blogs
                    .Where(current =>
                        current.Id != blog.Id && current.IsActive && !current.IsDeleted &&
                        current.BlogGroupId == blog.BlogGroupId)
                    .OrderByDescending(current => current.CreationDate).Take(3).ToList();

                blogDetail.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();

                ViewBag.Title = blog.Title;

                ViewBag.Description = blog.Summery;

                ViewBag.Canonical = "https://kamransehat.ir/blog/" + blog.BlogGroup.UrlParam + "/" + blog.UrlParam;

                return View(blogDetail);
            }

            return RedirectPermanent("/blog/" + blog.BlogGroup.UrlParam + "/" + urlParam);
        }
    }
}
