using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class CourseDetail:BaseEntity
    {
        public CourseDetail()
        {
            UserCourseDetails=new List<UserCourseDetail>();
        }
   

        [Display(Name="تاریخ برگزاری")]
        public DateTime PresentDate { get; set; }

        [Display(Name="اساتید")]
        public string Teachers { get; set; }

        [Display(Name="عنوان دوره")]
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<UserCourseDetail> UserCourseDetails { get; set; }

        internal class Configuration : EntityTypeConfiguration<CourseDetail>
        {
            public Configuration()
            {
                HasRequired(p => p.Course)
                    .WithMany(j => j.CourseDetails)
                    .HasForeignKey(p => p.CourseId);
            }
        }
    }
}