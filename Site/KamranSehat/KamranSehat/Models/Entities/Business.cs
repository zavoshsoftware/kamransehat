using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Business:BaseEntity
    {
        [Display(Name="نوع کسب و کار")]
        public string Type { get; set; }

        [Display(Name="صفحه اینستاگرام")]
        public string InstagramPage { get; set; }

        [Display(Name="ایمیل")]
        public string Email { get; set; }

        [Display(Name="وب سایت")]
        public string Website { get; set; }

        public Guid PackageId { get; set; }
        public virtual Package Package { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        internal class Configuration : EntityTypeConfiguration<Business>
        {
            public Configuration()
            {
                HasRequired(p => p.Package)
                    .WithMany(j => j.Businesses)
                    .HasForeignKey(p => p.PackageId);

                HasRequired(p => p.User)
                    .WithMany(j => j.Businesses)
                    .HasForeignKey(p => p.UserId);
            }
        }
    }
}