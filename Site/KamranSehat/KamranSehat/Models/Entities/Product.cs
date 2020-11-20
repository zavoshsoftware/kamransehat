using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Product:BaseEntity
    {
        public Product()
        {
            OrderDetails=new List<OrderDetail>();
            Rates=new List<Rate>();
            ProductComments=new List<ProductComment>();
        }
        [Display(Name = "Order", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Order { get; set; }

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

        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Amount { get; set; }

        [NotMapped]
        public string AmountStr
        {
            get { return Amount.ToString("n0")+" تومان"; }
        }

        [Display(Name = "DiscountAmount", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "IsInPromotion", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool IsInPromotion { get; set; }

        [Display(Name = "IsInHome", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool IsInHome { get; set; }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroup))]
        public Guid ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }

        [Display(Name="فایل نمونه ویدئو")]
        public string SampleVideoUrl { get; set; }

        [Display(Name="فایل اصلی ویدئو")]
        public string VideoUrl { get; set; }

        [Display(Name = "فایل پوستر ویدئو")]
        public string VideoPosterImageUrl { get; set; }


        [Display(Name = "مدت زمان")]
        public string Duration { get; set; }


        [Display(Name = "حجم فایل")]
        public string Size { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "موجودی")]
        public int Stock { get; set; }

        [Display(Name = "موجودی اولیه")]
        public int SeedStock { get; set; }

        [Display(Name = "کد محصولات مرتبط")]
        public string RelatedProductCodes { get; set; }

        public bool IsUpseller { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<ProductComment> ProductComments { get; set; }
        public int WorsthRate { get; set; }
        public int BestRate { get; set; }
        public decimal AverageRate { get; set; }
        public int RateCount { get; set; }
        internal class configuration : EntityTypeConfiguration<Product>
        {
            public configuration()
            {
                HasRequired(p => p.ProductGroup).WithMany(t => t.Products).HasForeignKey(p => p.ProductGroupId);
            }
        }
    }
}