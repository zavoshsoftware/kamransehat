using Resources;

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class User : BaseEntity
    {
        public User()
        {
            Orders = new List<Order>();
            UserCourseDetails = new List<UserCourseDetail>();
            Businesses = new List<Business>();
            Questions = new List<Question>();
        }


        [Display(Name = "Password", ResourceType = typeof(Resources.Models.User))]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "CellNum", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(20, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", ErrorMessageResourceName = "MobilExpersionValidation", ErrorMessageResourceType = typeof(Messages))]
        public string CellNum { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string FullName { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.User))]
        public int? Code { get; set; }

        [Column(TypeName = "datetime2")]
        [Display(Name = "BirthDate", ResourceType = typeof(Resources.Models.User))]
        public DateTime? BirthDate { get; set; }

        [NotMapped]
        public string BirthDateStr
        {
            get
            {
                // BirthDate= DateTime.SpecifyKind(BirthDate.Value, DateTimeKind.Utc);
                if (BirthDate.HasValue)
                {
                    string currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

                    if (currentCulture == "fa-IR")
                    {
                        System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                        string year = pc.GetYear(BirthDate.Value).ToString().PadLeft(4, '0');
                        string month = pc.GetMonth(BirthDate.Value).ToString().PadLeft(2, '0');
                        string day = pc.GetDayOfMonth(BirthDate.Value).ToString().PadLeft(2, '0');
                        return String.Format("{0}/{1}/{2}", year, month, day);
                    }
                    else if (currentCulture == "en-US")
                    {
                        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
                        string year = gc.GetYear(BirthDate.Value).ToString().PadLeft(4, '0');
                        string month = gc.GetMonth(BirthDate.Value).ToString().PadLeft(2, '0');
                        string day = gc.GetDayOfMonth(BirthDate.Value).ToString().PadLeft(2, '0');
                        return String.Format("{0}/{1}/{2}", year, month, day);
                    }
                    else
                    {
                        return "culture is not defined";
                    }
                }
                //if(BirthDate.HasValue)
                //{

                //    PersianCalendar oPersianCalendar = new PersianCalendar();
                //    DateTime date = Convert.ToDateTime(BirthDate);
                //    string year = oPersianCalendar.GetYear(date).ToString().PadLeft(4, '0');
                //    string month = oPersianCalendar.GetMonth(date).ToString().PadLeft(2, '0');
                //    string day = oPersianCalendar.GetDayOfMonth(date).ToString().PadLeft(2, '0');
                //     return System.String.Format("{0}/{1}/{2}", year, month, day);


                //}
                return string.Empty;
            }
            set
            {
                if (value != null)
                {
                    PersianCalendar c = new PersianCalendar();

                    string[] arr = value.Split('/');
                    int year = int.Parse(arr[2]);
                    int month = int.Parse(arr[1]);

                    if (year > 1000)
                    {
                        int day = int.Parse(arr[0]);

                        DateTime returnDate = c.ToDateTime(year, month, day, 0, 0, 0, 0);


                        BirthDate = returnDate;
                    }
                    else
                    {
                        year = int.Parse(arr[0]);
                        int day = int.Parse(arr[2]);

                        DateTime returnDate = c.ToDateTime(year, month, day, 0, 0, 0, 0);


                        BirthDate = returnDate;
                    }
                }
            }
        }

        [Display(Name = "Address", ResourceType = typeof(Resources.Models.User))]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "PostalCode", ResourceType = typeof(Resources.Models.User))]
        [StringLength(11, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string PostalCode { get; set; }

        [Display(Name = "AvatarImageUrl", ResourceType = typeof(Resources.Models.User))]
        //[StringLength(200, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string AvatarImageUrl { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.Models.User))]
        [StringLength(256, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "EmailExpersionValidation", ErrorMessageResourceType = typeof(Messages))]
        public string Email { get; set; }

        [Display(Name = "GenderTitle", ResourceType = typeof(Resources.Models.Gender))]
        public Guid? GenderId { get; set; }

        public Guid RoleId { get; set; }

        public virtual ICollection<UserCourseDetail> UserCourseDetails { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Business> Businesses { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}

