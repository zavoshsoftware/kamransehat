using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogDetailViewModel : BaseViewModel
    {
        public  Blog Blog { get; set; }
        public List<BlogGroup> BlogGroups { get; set; }
        public List<Blog> RecentBlogs { get; set; }
        public List<Blog> RelatedBlogs { get; set; }
        public List<BlogComment> BlogComments { get; set; }
    }
}