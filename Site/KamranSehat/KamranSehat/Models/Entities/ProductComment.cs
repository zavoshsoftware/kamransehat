using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class ProductComment : BaseEntity
    {
        [Display(Name = "نام")]
        [StringLength(200, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "ایمیل")]
        [StringLength(256, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Email { get; set; }

        [Display(Name = "پیغام")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "پاسخ")]
        [DataType(DataType.MultilineText)]
        public string Response { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public DateTime? ResponseDate { get; set; }

        [Display(Name = "نام پاسخ دهنده")]
        public string ResponseName { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        internal class Configuration : EntityTypeConfiguration<ProductComment>
        {
            public Configuration()
            {
                HasRequired(p => p.Product)
                    .WithMany(j => j.ProductComments)
                    .HasForeignKey(p => p.ProductId);
            }
        }


        [NotMapped]
        public string ResponseDateStr
        {
            get
            {
                if (ResponseDate != null)
                {
                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    string year = pc.GetYear(ResponseDate.Value).ToString().PadLeft(4, '0');
                    string month = pc.GetMonth(ResponseDate.Value).ToString().PadLeft(2, '0');
                    string day = pc.GetDayOfMonth(ResponseDate.Value).ToString().PadLeft(2, '0');
                    return String.Format("{0}/{1}/{2}", year, month, day);
                }
                return string.Empty;
            }
        }
    }
}