using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class QuestionCreateViewModel : BaseViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string FullName { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string CellNumber { get; set; }
        public string Subject { get; set; }

        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Body { get; set; }
        public string Email { get; set; }
    }
}