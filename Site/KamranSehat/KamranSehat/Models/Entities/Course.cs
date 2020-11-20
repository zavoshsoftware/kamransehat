using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Course : BaseEntity
    {
        public Course()
        {
            CourseDetails=new List<CourseDetail>();
        }

        [Display(Name="عنوان دوره")]
        public string Title { get; set; }

        [Display(Name="تصویر دوره")]
        public string ImageUrl { get; set; }

        [Display(Name="توضیحات کوتاه")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name="توضیحات دوره")]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        public virtual ICollection<CourseDetail> CourseDetails { get; set; }
    }
}