using System.ComponentModel.DataAnnotations;

namespace  ViewModels
{
    public class RegisterViewModel:BaseViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CellNumber { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FullName { get; set; }
    }
}