using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CallBackViewModel : BaseViewModel
    {
        public bool IsSuccess { get; set; }
        public string RefrenceId { get; set; }
        public string OrderCode { get; set; }
        public List<Product> Products { get; set; }
        public string CreationDate { get; set; }
        public string ProductType { get; set; }
    }

   
}