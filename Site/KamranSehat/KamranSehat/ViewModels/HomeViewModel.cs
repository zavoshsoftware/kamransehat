using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        //public List<Product> PopularProducts { get; set; }
        public List<Blog> RecentBlog { get; set; }
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<TeamMember> TeamMembers { get; set; }

    }
}