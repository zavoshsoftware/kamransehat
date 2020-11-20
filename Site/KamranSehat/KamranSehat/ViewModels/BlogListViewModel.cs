using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogListViewModel : BaseViewModel
    {
        public string BlogGroupTitle { get; set; }
        public string UrlParam { get; set; }
        public string BlogGroupSummery { get; set; }
        public List<Blog> Blogs { get; set; }
    
    }
}