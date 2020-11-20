using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class BlogGroup : BaseEntity
    {
        public string Title { get; set; }
        public string Summery { get; set; }
        public string ImageUrl { get; set; }
        public string UrlParam { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }

    }
}