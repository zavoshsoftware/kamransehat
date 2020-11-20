using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ServiceGroupDetailViewModel : BaseViewModel
    {
        public ServiceGroup ServiceGroup { get; set; }
        public List<Service> Services { get; set; }
    }
}