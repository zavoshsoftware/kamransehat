using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Helpers;
using Models;
using ViewModels;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var products = db.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate).Include(p => p.ProductGroup).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate);
            return View(products.ToList());
        }

        [AllowAnonymous]
        [Route("product/{productGroupUrlParam}/{code}")]
        public ActionResult Details(string productGroupUrlParam, string code)
        {
            ViewBag.Canonical = "https://kamransehat.ir/product/" + productGroupUrlParam + "/" + code;

            Product product = db.Products.FirstOrDefault(current => current.Code == code && current.IsDeleted == false);

            //invalid product code
            if (product == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("AccessDenied", "Home");
            }

            //Invalid Product Group
            if (product.ProductGroup.UrlParam != productGroupUrlParam)
                return RedirectPermanent("/product/" + product.ProductGroup.UrlParam + "/" + product.Code);


            List<Product> relatedProduct = GetRelatedProduct(product);

            List<ProductComment> comments = db.ProductComments
                .Where(current => current.ProductId == product.Id && current.IsActive && current.IsDeleted == false)
                .OrderByDescending(current => current.CreationDate).ToList();


            ProductDetailViewModel productDetail = new ProductDetailViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),

                MenuServiceGroups = baseViewModel.GetMenuServices(),

                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),

                Product = product,

                RelatedProducts = relatedProduct,

                ProductComments = comments,

                CommentMessage = GetProductCommentMessage(comments.Count())
            };

            ViewBag.Title = product.PageTitle;
            ViewBag.Description = product.PageDescription;
            ViewBag.ogImage = "https://kamransehat.ir" + product.ImageUrl;
            ViewBag.ogType = "product";
            ViewBag.ogSitename = "وب سایت شخصی دکتر صحت";

            return View(productDetail);
        }

        public List<Product> GetRelatedProduct(Product currentProduct)
        {
            List<Product> relatedProduct = new List<Product>();

            if (string.IsNullOrEmpty(currentProduct.RelatedProductCodes))
            {
                relatedProduct = db.Products
                    .Where(current => current.IsUpseller && current.IsDeleted == false && current.IsActive&&current.Id!=currentProduct.Id)
                    .OrderByDescending(current => current.Code).Take(4).ToList();
            }
            else
            {
                string[] relatedCodes = currentProduct.RelatedProductCodes.Split(',');

                if (relatedCodes == null || relatedCodes.Length == 0)
                {
                }
                else
                {
                    for (int i = 0; i < relatedCodes.Length; i++)
                    {
                        string code = relatedCodes[i];
                        Product product = db.Products.FirstOrDefault(current =>
                            current.Code == code && current.IsActive && !current.IsDeleted);

                        relatedProduct.Add(product);
                    }
                }

                if (relatedProduct.Count() < 4)
                {
                    List<Product> extraRelatedProduct = db.Products
                        .Where(current => current.IsUpseller && current.IsDeleted == false && current.IsActive)
                        .OrderByDescending(current => current.Code).Take(4- relatedProduct.Count()).ToList();

                    foreach (Product product in extraRelatedProduct)
                    {
                        relatedProduct.Add(product);
                    }
                }

                relatedProduct = relatedProduct.OrderByDescending(current => current.Code).ToList();
            }

            return relatedProduct;
        }

        [AllowAnonymous]
        public string GetProductCommentMessage(int count)
        {
            if (count == 0)
                return "اولین نفری باشید که برای این ویدیو نظر خود را ثبت می کنید.";

            else
                return "تا کنون " + count + " نظر ثبت شده است.";
        }
        public ActionResult Create()
        {
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadSampleVideoUrl, HttpPostedFileBase fileUploadVideoUrl, HttpPostedFileBase fileUploadVideoPosterImageUrl)
        {


            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    product.ImageUrl = newFilenameUrl;
                }

                if (fileUploadVideoPosterImageUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadVideoPosterImageUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/Video/Poster/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUploadVideoPosterImageUrl.SaveAs(physicalFilename);

                    product.ImageUrl = newFilenameUrl;
                }

                if (fileUploadSampleVideoUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadSampleVideoUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/Video/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUploadSampleVideoUrl.SaveAs(physicalFilename);

                    product.SampleVideoUrl = newFilenameUrl;
                }

                if (fileUploadVideoUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadVideoUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Videos/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUploadVideoUrl.SaveAs(physicalFilename);

                    product.VideoUrl = newFilenameUrl;
                }
                #endregion
                product.IsDeleted = false;
                product.CreationDate = DateTime.Now;
                product.Code = ReturnCode();
                product.Id = Guid.NewGuid();
                product.AverageRate = 5;
                product.BestRate = 5;
                product.WorsthRate = 5;
                product.RateCount = 1;
                product.LastModifiedDate = DateTime.Now;

                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }


            ViewBag.ProductGroupId = new SelectList(db.ProductGroups.Where(current => current.IsDeleted == false), "Id", "Title", product.ProductGroupId);
            return View(product);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadSampleVideoUrl, HttpPostedFileBase fileUploadVideoUrl, HttpPostedFileBase fileUploadVideoPosterImageUrl)
        {
            if (ModelState.IsValid)
            {


                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    product.ImageUrl = newFilenameUrl;
                }

                if (fileUploadVideoPosterImageUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadVideoPosterImageUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/Video/Poster/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUploadVideoPosterImageUrl.SaveAs(physicalFilename);

                    product.ImageUrl = newFilenameUrl;
                }
                if (fileUploadSampleVideoUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadSampleVideoUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/Video/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUploadSampleVideoUrl.SaveAs(physicalFilename);

                    product.SampleVideoUrl = newFilenameUrl;
                }

                if (fileUploadVideoUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadVideoUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Videos/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUploadVideoUrl.SaveAs(physicalFilename);

                    product.VideoUrl = newFilenameUrl;
                }
                #endregion
                product.LastModifiedDate = DateTime.Now;
                product.IsDeleted = false;
                db.Entry(product).State = EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups.Where(current => current.IsDeleted == false), "Id", "Title", product.ProductGroupId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Find(id);
            product.IsDeleted = true;
            product.DeletionDate = DateTime.Now;

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

        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();

        [AllowAnonymous]
        [Route("product/{urlParam}")]
        public ActionResult List(string urlParam)
        {
            ProductGroup productGroup = db.ProductGroups.FirstOrDefault(current => current.UrlParam == urlParam);

            if (productGroup == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("AccessDenied", "Home");
            }

            ProductListViewModel product = new ProductListViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),

                Products =
                    db.Products.Where(current =>
                         current.ProductGroupId == productGroup.Id && current.IsDeleted == false && current.IsActive)
                        .OrderByDescending(current => current.Order).ToList()
            };
            ViewBag.Canonical = "https://kamransehat.ir/product/" + urlParam;
            return View(product);
        }
        [AllowAnonymous]
        public string ReturnCode()
        {

            Product product = db.Products.OrderByDescending(current => current.Code).FirstOrDefault();
            if (product != null)
            {
                return (Convert.ToInt32(product.Code) + 1).ToString();
            }
            else
            {
                return "1000";
            }
        }
        [AllowAnonymous]
        [Route("ShowProduct/{productCode}/{orderCode:int}")]
        public ActionResult ShowProduct(string productCode, int orderCode)
        {
            Product product = db.Products.FirstOrDefault(current => current.Code == productCode);

            bool isLogin = HttpContext.User.Identity.IsAuthenticated;
            if (!isLogin)
            {
                return Redirect("/account/login?ReturnUrl=/ShowProduct/" + productCode + "/" + orderCode);
            }

            User user = db.Users.FirstOrDefault(current => current.CellNum == HttpContext.User.Identity.Name);

            if (user == null)
            {
                return Redirect("/account/login?ReturnUrl=/ShowProduct/" + productCode + "/" + orderCode);
            }

            OrderDetail detail = db.OrderDetails.FirstOrDefault(current => current.ProductId == product.Id &&
            current.Order.Code == orderCode && current.Order.UserId == user.Id);

            if (detail == null)
            {
                Response.StatusCode = 404;
                return Redirect("/AccessDenied");
            }

            ShowProductViewModel showProductViewModel = new ShowProductViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                Product = product
            };

            return View(showProductViewModel);
        }


        [Route("Download/{code}")]
        [AllowAnonymous]
        public ActionResult Download(string code)
        {
            Product product = db.Products.FirstOrDefault(current => current.Code == code);

            bool isLogin = HttpContext.User.Identity.IsAuthenticated;

            if (!isLogin)
            {
                return Redirect("/account/login?ReturnUrl=/Download/" + product.Code);
            }

            User user = db.Users.FirstOrDefault(current => current.CellNum == HttpContext.User.Identity.Name);

            if (user == null)
            {
                return Redirect("/account/login?ReturnUrl=/Download/" + product.Code);
            }

            OrderDetail detail = db.OrderDetails.FirstOrDefault(current =>
                current.ProductId == product.Id && current.Order.UserId == user.Id);

            if (detail == null)
            {
                Response.StatusCode = 404;
                return Redirect("/AccessDenied");
            }

            string fileLocation = Server.MapPath(product.VideoUrl);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileLocation);
            string fileName = "kamransehat-" + code + ".mp4";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        [AllowAnonymous]
        [Route("product")]
        public ActionResult AllProducts()
        {
            return RedirectPermanent("/product/video");

            //ProductListViewModel product = new ProductListViewModel()
            //{
            //    MenuProductGroups = baseViewModel.GetMenu(),
            //    FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
            //    MenuServiceGroups = baseViewModel.GetMenuServices(),

            //    Products =
            //        db.Products.Where(current => current.IsDeleted == false && current.IsActive)
            //            .OrderByDescending(current => current.Stock).ThenByDescending(current => current.Order).ToList()
            //};
            //ViewBag.Canonical = "https://www.drsehat.ir/Allproducts/";
            //return View(product);
        }
    }
}
