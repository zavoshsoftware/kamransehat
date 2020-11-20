using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class ServiceGroup:BaseEntity
    {
        public ServiceGroup()
        {
            Services = new List<Service>();
        }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "اولویت")]
        public int Order { get; set; }
        [Display(Name = "تضویر")]
        public string ImageUrl { get; set; }
        [Display(Name = "متن")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}