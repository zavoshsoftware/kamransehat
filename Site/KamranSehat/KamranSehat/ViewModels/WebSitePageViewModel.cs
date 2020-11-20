using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class WebSitePageViewModel:BaseViewModel
    {
        public ProductGroup ProductGroup { get; set; }
        public List<Product> Products { get; set; }
    }
}