﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CheckOutViewModel : BaseViewModel
    {
        public bool IsUseDiscount { get; set; }
     //   public List<ProductInCart> Products { get; set; }
        public string ShippingAmount { get; set; }
        public string Total { get; set; }
        public string SubTotal { get; set; }
        public string DiscountAmount { get; set; }
        public List<Province> Provinces { get; set; }
        public UserInformation UserInformation { get; set; }
    }

    public class UserInformation
    {
        public string FullName { get; set; }
        public Guid CityId { get; set; }
        public string CellNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
    }
}