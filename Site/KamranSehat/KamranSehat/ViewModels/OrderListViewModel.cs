using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderListViewModel : BaseViewModel
    {
        public List<OrderItemViewModel> Orders { get; set; }
        public bool HasOrder { get; set; }
    }

    public class OrderItemViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public string Total { get; set; } 
    }
}