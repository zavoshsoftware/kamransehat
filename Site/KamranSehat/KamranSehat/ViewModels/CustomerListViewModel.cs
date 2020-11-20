using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CustomerListViewModel:BaseViewModel
    {
        public List<CustomerGroup> CustomerGroups { get; set; }
        public List<Customer> Customers { get; set; }
        //public List<CustomerWithGroup> CustomerList { get; set; }
    }
    //public class CustomerWithGroup
    //{
    //    public CustomerGroup CustomerGroup { get; set; }
    //    public List<Customer> Customers { get; set; }
    //}
}