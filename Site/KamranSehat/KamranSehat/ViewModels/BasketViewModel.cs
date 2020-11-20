using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BasketViewModel : BaseViewModel
    {
        public List<Product> Products { get; set; }
        public string SubTotal { get; set; }
        public string DiscountAmount { get; set; }
        public decimal Total { get; set; }
    }

    public class ProductInCart
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string RowAmount
        {
            get
            {
                return (Product.Amount * Quantity).ToString("n0");
            }
        }
    }
}