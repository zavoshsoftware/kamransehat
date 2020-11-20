using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Book:BaseEntity
    {
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Product))]
        [StringLength(15, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Code { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(256, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "PageTitle", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string PageTitle { get; set; }

        [Display(Name = "PageDescription", ResourceType = typeof(Resources.Models.Product))]
        [StringLength(1000, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string PageDescription { get; set; }

        [Display(Name = "ImageUrl", ResourceType = typeof(Resources.Models.Product))]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string ImageUrl { get; set; }

        [Display(Name = "Summery", ResourceType = typeof(Resources.Models.Product))]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "Body", ResourceType = typeof(Resources.Models.Product))]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        [Display(Name = "IsInHome", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool IsInHome { get; set; }

        [Display(Name = "فایل اصلی کتاب")]
        public string BookUrl { get; set; }

        [Display(Name = "حجم فایل")]
        public string Size { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
        public int WorsthRate { get; set; }
        public int BestRate { get; set; }
        public decimal AverageRate { get; set; }
        public int RateCount { get; set; }
    }
}