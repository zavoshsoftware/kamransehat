
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Consultation:BaseEntity
    {
        [Display(Name = "Fullname", ResourceType = typeof(Resources.Models.Consultation))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Fullname { get; set; }

        [Display(Name = "Organization", ResourceType = typeof(Resources.Models.Consultation))]
        public string Organization { get; set; }

        [Display(Name = "Position", ResourceType = typeof(Resources.Models.Consultation))]
        public string Position { get; set; }

        [Display(Name = "Front", ResourceType = typeof(Resources.Models.Consultation))]
        public string Front { get; set; }

        [Display(Name = "CellNumber", ResourceType = typeof(Resources.Models.Consultation))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(20, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", ErrorMessageResourceName = "MobilExpersionValidation", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string CellNumber { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.Models.Consultation))]
        [StringLength(256, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "EmailExpersionValidation", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Email { get; set; }

        [Display(Name = "Website", ResourceType = typeof(Resources.Models.Consultation))]
        public string Website { get; set; }

        [Display(Name = "Body", ResourceType = typeof(Resources.Models.Consultation))]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        [Display(Name = "DayOfWeek", ResourceType = typeof(Resources.Models.Consultation))]
        public string DayOfWeek { get; set; }

        [Display(Name = "Time", ResourceType = typeof(Resources.Models.Consultation))]
        public string Time { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.Models.Consultation))]
        public string Type { get; set; }
    }
}