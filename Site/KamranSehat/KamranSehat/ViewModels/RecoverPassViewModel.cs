using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class RecoverPassViewModel : BaseViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Username { get; set; }
    }
}