using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class BusinessCreateViewModel:BaseViewModel
    {
        public string FullName { get; set; }
        public string CellNumber { get; set; }
        public string Type { get; set; }
        public string InstagramPage { get; set; }
        public string Website { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Email { get; set; }
        public Guid PackageId { get; set; }
    }
}