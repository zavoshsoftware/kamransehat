using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Package:BaseEntity
    {
        public Package()
        {
            Businesses=new List<Business>();
        }
        public string Title { get; set; }
        public int Duration { get; set; }
        public decimal Amount { get; set; }

        public virtual ICollection<Business> Businesses { get; set; }
    }
}