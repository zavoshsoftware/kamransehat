using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductCopyViewModel
    {
        public Guid  Id { get; set; }
        [Display(Name = "تعداد کپی")]
        public int Copy { get; set; }
        public Product Product { get; set; }
    }
}