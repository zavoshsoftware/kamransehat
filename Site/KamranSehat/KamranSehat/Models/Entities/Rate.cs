using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Rate : BaseEntity
    {
        public int Value { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        internal class configuration : EntityTypeConfiguration<Rate>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.Rates).HasForeignKey(p => p.ProductId);
            }
        }
    }
}