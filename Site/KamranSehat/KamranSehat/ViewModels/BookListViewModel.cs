using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BookListViewModel:BaseViewModel
    {
        public List<Book> Books { get; set; }
    }
}