using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class RadioListViewModel:BaseViewModel
    {
        public List<Radio> RadioList { get; set; }
    }
}