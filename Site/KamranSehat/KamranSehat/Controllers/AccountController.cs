using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Eshop.Helpers;
using Helpers;
using ViewModels;

namespace KamranSehat.Controllers
{
    public class AccountController : Controller
    {
        BaseViewModelHelper baseHelper = new BaseViewModelHelper();

        // GET: Account
        private DatabaseContext db = new DatabaseContext();
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;

            LoginViewModel login = new LoginViewModel();
            login.FooterRecentBlog = baseHelper.GetFooterRecentBlog();
            login.MenuServiceGroups = baseHelper.GetMenuServices();
            login.MenuProductGroups = baseHelper.GetMenu();

            return View(login);
        }
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User oUser = db.Users.Where(a => a.CellNum == model.Username && a.Password == model.Password).FirstOrDefault();

                if (oUser != null)
                {
                    var ident = new ClaimsIdentity(
                      new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,oUser.CellNum),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, oUser.Role.Name),

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = true }, ident);
                    return RedirectToLocal(returnUrl, oUser.Role.Name); // auth succeed 
                }
                else
                {
                    // invalid username or password
                    TempData["WrongPass"] = "نام کاربری و یا کلمه عبور وارد شده صحیح نمی باشد.";
                }
            }

            LoginViewModel login = new LoginViewModel();
            login.FooterRecentBlog = baseHelper.GetFooterRecentBlog();
            login.MenuServiceGroups = baseHelper.GetMenuServices();
            login.MenuProductGroups = baseHelper.GetMenu();
            return View(login);
        }

        public ActionResult Register()
        {
            RegisterViewModel register = new RegisterViewModel();
            register.FooterRecentBlog = baseHelper.GetFooterRecentBlog();
            register.MenuServiceGroups = baseHelper.GetMenuServices();
            register.MenuProductGroups = baseHelper.GetMenu();

            return View(register);
        }

        private CodeCreator codeCreator = new CodeCreator();
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel oRegisterViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isValidMobile = Regex.IsMatch(oRegisterViewModel.CellNumber, @"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", RegexOptions.IgnoreCase);

                if (!isValidMobile)
                    ModelState.AddModelError("invalidMobile", "شماره موبایل وارد شده صحیح نمی باشد");


                else
                {

                    User user = db.Users.FirstOrDefault(current =>
                        current.CellNum == oRegisterViewModel.CellNumber && current.IsActive && !current.IsDeleted);

                    if (user != null)
                    {
                        ModelState.AddModelError("duplicateMobil", "این شماره موبایل قبلا در وب سایت ثبت شده است. می توانید از قسمت ورود، وارد سایت شوید.");

                    }
                    else
                    {
                        Guid roleId = db.Roles.FirstOrDefault(current => current.Name.ToLower() == "customer").Id;

                        User oUser = new User();

                        oUser.Id = Guid.NewGuid();
                        oUser.FullName = oRegisterViewModel.FullName;
                        oUser.IsDeleted = false;
                        oUser.IsActive = false;
                        oUser.CellNum = oRegisterViewModel.CellNumber;
                        oUser.Email = oRegisterViewModel.Email;
                        oUser.Password = RandomCode().ToString();
                        oUser.CreationDate = DateTime.Now;
                        oUser.RoleId = roleId;
                        oUser.Code = codeCreator.ReturnUserCode();

                        db.Users.Add(oUser);
                        db.SaveChanges();

                        return RedirectToAction("activate",new{id=oUser.Code});
                    }

                }
                
            }
            RegisterViewModel register = new RegisterViewModel();
            register.FooterRecentBlog = baseHelper.GetFooterRecentBlog();
            register.MenuServiceGroups = baseHelper.GetMenuServices();
            register.MenuProductGroups = baseHelper.GetMenu();

            return View(register);
        }
        [AllowAnonymous]
        public ActionResult Activate(int id)
        {

            ActivateUserViewModel businessViewModel = new ActivateUserViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                UserCode = id,
            };

            return View(businessViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Activate(ActivateUserViewModel activeCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = db.Users.FirstOrDefault(current => current.Code == activeCode.UserCode && current.Password == activeCode.ActivateCode);

                    if (user == null)
                        ModelState.AddModelError("invalidCode", "کد وارد شده صحیح نمی باشد");
                    else
                    {
                        user.IsActive = true;
                        user.LastModifiedDate = DateTime.Now;

                        db.SaveChanges();
                        TempData["success"] = "با تشکر. حساب کاربری شما در وب سایت دکتر کامران صحت با موفقیت فعال شد.";

                        activeCode.MenuProductGroups = baseViewModel.GetMenu();
                        activeCode.MenuServiceGroups = baseViewModel.GetMenuServices();
                        activeCode.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();
                         
                        return View(activeCode);

                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("catchError", "مشکلی در پردازش اطلاعات به وجود آمده است. لطفا دوباره وارد سایت شوید.");

            }
            ActivateUserViewModel businessViewModel = new ActivateUserViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
            };

            return View(businessViewModel);
        }
        private ActionResult RedirectToLocal(string returnUrl, string role)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (role.ToLower() == "administrator")
                    return RedirectToAction("Index", "blogs");

                return RedirectToAction("index", "home");
            }
        }
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
            return RedirectToAction("index","Home");
        }
        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }
        public ActionResult RecoverPass()
        {
            RecoverPassViewModel recoverPass = new RecoverPassViewModel();
            recoverPass.FooterRecentBlog = baseHelper.GetFooterRecentBlog();
            recoverPass.MenuServiceGroups = baseHelper.GetMenuServices();
            recoverPass.MenuProductGroups = baseHelper.GetMenu();

            return View(recoverPass);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RecoverPass(RecoverPassViewModel model)
        {
            if (ModelState.IsValid)
            {
                User oUser = db.Users.FirstOrDefault(a => a.CellNum == model.Username);

                if (oUser != null)
                {
                    Sms sms = new Sms();
                    sms.SendSmsForRecoverPassword(oUser.CellNum,oUser.Password);
                    return RedirectToAction("login");
                }
                else
                {
                    TempData["WrongPass"] = "کاربری با این شماره موبایل در سایت وجود ندارد.";
                }
            }

            RecoverPassViewModel recoverPass = new RecoverPassViewModel();
            recoverPass.FooterRecentBlog = baseHelper.GetFooterRecentBlog();
            recoverPass.MenuServiceGroups = baseHelper.GetMenuServices();
            recoverPass.MenuProductGroups = baseHelper.GetMenu();

            return View(recoverPass);
        }
    }
}