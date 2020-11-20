using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class ContactForm:BaseEntity
    {
        [Display(Name="نام")]
        public string Name { get; set; }

        [Display(Name="ایمیل")]
        public string Email { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name="پیغام")]
        public string Message { get; set; }
    }
}