using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ActivateUserViewModel : BaseViewModel
    {

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string ActivateCode { get; set; }

        public int UserCode { get; set; }
        public Guid QuestionId { get; set; }
    }
}