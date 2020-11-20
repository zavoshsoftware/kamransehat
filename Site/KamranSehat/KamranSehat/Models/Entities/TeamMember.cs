using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TeamMember:BaseEntity
    {
        [Display(Name = "اولویت")]
        public int Order { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name = "سمت")]
        public string Post { get; set; }
        [Display(Name = "آدرس Linkedin")]
        public string LinkedinUrl { get; set; }
        [Display(Name = "آدرس Instagram")]
        public string InstagramUrl { get; set; }
        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }
    }
}