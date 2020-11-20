using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductDetailViewModel : BaseViewModel
    {
        public Product Product { get; set; }
     
        public List<Product> RelatedProducts { get; set; }
        public List<Product> UpSellroducts { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public string CommentMessage { get; set; }
    }
}