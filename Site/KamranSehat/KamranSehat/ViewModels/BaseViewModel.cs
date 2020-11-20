using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BaseViewModel
    {
        public List<MenuProductGroup> MenuProductGroups { get; set; }
        public List<MenuServiceGroup> MenuServiceGroups { get; set; }
        public List<Blog> FooterRecentBlog { get; set; }
    }

    public class MenuProductGroup
    {
        public ProductGroup ParentProductGroup { get; set; }
        public List<ProductGroup> ChildProductGroups { get; set; }
    }

    public class MenuServiceGroup
    {
        public ServiceGroup ServiceGroup { get; set; }
        public List<Service> Services { get; set; }
    }
}