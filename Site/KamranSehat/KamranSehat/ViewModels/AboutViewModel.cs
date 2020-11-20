using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public List<Product> products { get; set; }
    }
}