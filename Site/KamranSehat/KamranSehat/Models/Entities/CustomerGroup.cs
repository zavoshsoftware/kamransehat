using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CustomerGroup : BaseEntity
    {
        public CustomerGroup()
        {
            Customers = new List<Customer>();
        }
        public int Order { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}