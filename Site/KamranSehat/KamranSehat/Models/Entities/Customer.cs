using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Customer:BaseEntity
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public Guid CustomerGroupId { get; set; }
        public virtual CustomerGroup CustomerGroup { get; set; }

        internal class Configuration : EntityTypeConfiguration<Customer>
        {
            public Configuration()
            {
                HasOptional(p => p.CustomerGroup)
                    .WithMany(j => j.Customers)
                    .HasForeignKey(p => p.CustomerGroupId);
            }
        }
    }
}