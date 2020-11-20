using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using ViewModels;

namespace Helpers
{
    public class BaseViewModelHelper
    {
        private DatabaseContext db = new DatabaseContext();
        public List<MenuProductGroup> GetMenu()
        {
            List<MenuProductGroup> menu = new List<MenuProductGroup>();

            List<ProductGroup> productGroups = db.ProductGroups
                .Where(current => current.IsDeleted == false && current.IsActive && current.ParentId == null).OrderBy(current => current.Order)
                .ToList();

            foreach (ProductGroup productGroup in productGroups)
            {
                menu.Add(new MenuProductGroup()
                {
                    ParentProductGroup = productGroup,
                    ChildProductGroups = db.ProductGroups.Where(current => current.ParentId == productGroup.Id && current.IsActive && !current.IsDeleted).OrderBy(current => current.Order).ToList()
                });
            }

            return menu;
        }

        public List<MenuServiceGroup> GetMenuServices()
        {
            List<MenuServiceGroup> menu = new List<MenuServiceGroup>();

            List<ServiceGroup> serviceGroups = db.ServiceGroups
                .Where(current => current.IsDeleted == false && current.IsActive).OrderBy(current => current.Order)
                .ToList();

            foreach (ServiceGroup serviceGroup in serviceGroups)
            {
                menu.Add(new MenuServiceGroup()
                {
                    ServiceGroup = serviceGroup,
                    Services = db.Services.Where(current => current.ServiceGroupId == serviceGroup.Id && current.IsActive && !current.IsDeleted).OrderBy(current => current.Order).ToList()
                });
            }

            return menu;
        }


        public List<Blog> GetFooterRecentBlog()
        {
            List<Blog> blogs = db.Blogs.Where(current => current.IsDeleted == false && current.IsActive)
                .OrderByDescending(current => current.CreationDate).Take(2).ToList();

            return blogs;
        }
    }
}