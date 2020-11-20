using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class UserCourseDetail : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid CourseDetailId { get; set; }
        public User User { get; set; }
        public CourseDetail CourseDetail { get; set; }

        internal class Configuration : EntityTypeConfiguration<UserCourseDetail>
        {
            public Configuration()
            {
                HasRequired(p => p.User)
                    .WithMany(j => j.UserCourseDetails)
                    .HasForeignKey(p => p.UserId);

                HasRequired(p => p.CourseDetail)
                    .WithMany(j => j.UserCourseDetails)
                    .HasForeignKey(p => p.CourseDetailId);
            }
        }
    }
}