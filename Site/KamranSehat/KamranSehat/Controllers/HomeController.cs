using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace KamranSehat.Controllers
{
    public class HomeController : Controller
    {
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        private DatabaseContext db = new DatabaseContext();

        [Route("")]
        public ActionResult Index()
        {
            HomeViewModel home = new HomeViewModel();
            home.MenuProductGroups = baseViewModel.GetMenu();
            home.MenuServiceGroups = baseViewModel.GetMenuServices();
            home.Products = db.Products.Where(current => current.IsDeleted == false && current.IsActive == true && current.IsInHome == true).Take(3).ToList();
            home.Blogs = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive)
                .OrderByDescending(current => current.CreationDate).Take(4).ToList();
            home.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();
            home.TeamMembers = db.TeamMembers.Where(current => current.IsDeleted == false && current.IsActive == true).OrderBy(current => current.Order).ToList();
            return View(home);
        }
        [Route("AccessDenied")]
        public ActionResult AccessDenied()
        {

            BaseViewModel baseModel = new BaseViewModel()
            {
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                MenuProductGroups = baseViewModel.GetMenu()
            };
            return View(baseModel);
        }

        [Route("about")]
        public ActionResult About()
        {
            AboutViewModel about = new AboutViewModel()
            {
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                MenuProductGroups = baseViewModel.GetMenu(),
                products = db.Products.Where(current => current.ProductGroup.UrlParam.ToLower() == "video" && current.IsDeleted == false && current.IsActive).OrderByDescending(current => current.CreationDate).Take(3).ToList()
            };

            return View(about);
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            BaseViewModel baseModel = new BaseViewModel()
            {
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                MenuProductGroups = baseViewModel.GetMenu()
            };
            return View(baseModel);
        }

        [Route("websiteservice/{urlParam}")]
        public ActionResult WebSiteService(string urlParam)
        {

            WebSitePageViewModel website = new WebSitePageViewModel()
            {
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                MenuProductGroups = baseViewModel.GetMenu(),
                ProductGroup = db.ProductGroups.FirstOrDefault(c=>c.UrlParam==urlParam),
                Products = db.Products.Where(c=>c.ProductGroup.UrlParam==urlParam).ToList()
            };
            return View(website);
        }

        public ActionResult redirecttopayment()
        {

            return Redirect("https://Zarinp.al/280903");
        }
    }
}