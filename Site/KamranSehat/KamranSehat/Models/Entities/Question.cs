using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Question:BaseEntity
    {
        public Guid  UserId { get; set; }
        public virtual User User { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Response { get; set; }

        public Guid? OrderId { get; set; }
        public virtual Order Order { get; set; }


        internal class Configuration : EntityTypeConfiguration<Question>
        {
            public Configuration()
            {
                HasOptional(p => p.Order)
                    .WithMany(j => j.Questions)
                    .HasForeignKey(p => p.OrderId);

                HasRequired(p => p.User)
                    .WithMany(j => j.Questions)
                    .HasForeignKey(p => p.UserId);
            }
        }
    }
}