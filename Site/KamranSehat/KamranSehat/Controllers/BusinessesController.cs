using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Eshop.Helpers;
using Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models;
using ViewModels;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BusinessesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();

        public ActionResult Index()
        {
            return View(db.Businesses.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }



        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,InstagramPage,Email,Website,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] Business business)
        {
            if (ModelState.IsValid)
            {
                business.IsDeleted = false;
                db.Entry(business).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(business);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Business business = db.Businesses.Find(id);
            business.IsDeleted = true;
            business.DeletionDate = DateTime.Now;

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
        public ActionResult Activate(int id,Guid businessCode)
        {

            ActivateUserViewModel businessViewModel = new ActivateUserViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
            };
            ViewBag.PackageId = new SelectList(db.Packages.Where(current => current.IsActive), "Id", "Title");

            return View(businessViewModel);
        }

        #region Create




        [AllowAnonymous]
        public ActionResult Create()
        {

            BusinessCreateViewModel businessViewModel = new BusinessCreateViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
            };
            ViewBag.PackageId = new SelectList(db.Packages.Where(current => current.IsActive), "Id", "Title");

            return View(businessViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(BusinessCreateViewModel business)
        {
            try
            {

            if (ModelState.IsValid)
            {
                bool isValidMobile = Regex.IsMatch(business.CellNumber, @"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", RegexOptions.IgnoreCase);

                if (!isValidMobile)
                    ModelState.AddModelError("invalidMobile", "شماره موبایل وارد شده صحیح نمی باشد");

                else
                {
                   User user = InsertUser(business.FullName, business.CellNumber);
                    db.SaveChanges();
                 Guid businessId=   InsertBusiness(user.Id, business.Email, business.InstagramPage, business.Type, business.PackageId,
                        business.Description);

                    db.SaveChanges();

                    return RedirectToAction("Activate", new {id = user.Code, businessCode = businessId});
                }

            }

            business.MenuProductGroups = baseViewModel.GetMenu();
            business.MenuServiceGroups = baseViewModel.GetMenuServices();
            business.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();
            ViewBag.PackageId = new SelectList(db.Packages.Where(current => current.IsActive), "Id", "Title");


            return View(business);

            }
            catch (Exception e)
            {
                BusinessCreateViewModel businessViewModel = new BusinessCreateViewModel()
                {
                    MenuProductGroups = baseViewModel.GetMenu(),
                    MenuServiceGroups = baseViewModel.GetMenuServices(),
                    FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
                };
                ViewBag.PackageId = new SelectList(db.Packages.Where(current => current.IsActive), "Id", "Title");

                return View(businessViewModel);
            }
        }
        Sms sms = new Sms();
        public User InsertUser(string fullname, string mobile)
        {
            User oUser = db.Users.FirstOrDefault(current => current.CellNum == mobile && current.IsDeleted == false);
            CodeCreator codeCreator=new CodeCreator();

            if (oUser == null)
            {
                User user = new User();
                user.Id = Guid.NewGuid();
                user.FullName = fullname;
                user.CellNum = mobile;
                user.Password = RandomCode().ToString();
                user.IsActive = false;
                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;
                user.RoleId = db.Roles.Where(current => current.Name.ToLower() == "customer").FirstOrDefault().Id;
                user.Code = codeCreator.ReturnUserCode();

                db.Users.Add(user);

                sms.SendSms(mobile, user.Password);
                //LoginUser(user);

                return user;

            }
            else
            {
                oUser.Password = RandomCode().ToString();
                oUser.LastModifiedDate = DateTime.Now;
                oUser.FullName = fullname;

                if (oUser.Code == null)
                    oUser.Code = codeCreator.ReturnUserCode();

                sms.SendSms(mobile, oUser.Password);
                //   LoginUser(oUser);
                return oUser;
            }

            db.SaveChanges();

        }
        public void LoginUser(User user)
        {
            var ident = new ClaimsIdentity(
                new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.CellNum),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                    new Claim(ClaimTypes.Name, user.CellNum),

                    // optionally you could add roles if any
                    new Claim(ClaimTypes.Role, user.Role.Name),

                },
                DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties { IsPersistent = true }, ident);
        }
        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }

        public Guid InsertBusiness(Guid userId, string email, string pageAddress, string type, Guid packageId,
            string description)
        {
            Business business=new Business()
            {
                UserId = userId,
                Email = email,
                InstagramPage = pageAddress,
                Type = type,
                 PackageId = packageId,
                 Description = description,
                 CreationDate = DateTime.Now,
                 IsDeleted = false,
                 IsActive = false,
                 Id = Guid.NewGuid()
            };

            db.Businesses.Add(business);

            return business.Id;
        }
        #endregion
    }
}
