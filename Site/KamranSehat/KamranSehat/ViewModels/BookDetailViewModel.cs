using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BookDetailViewModel:BaseViewModel
    {
        public Book Book { get; set; }
    }
}