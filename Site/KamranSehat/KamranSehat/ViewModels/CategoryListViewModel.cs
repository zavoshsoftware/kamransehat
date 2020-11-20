using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CategoryListViewModel : BaseViewModel
    {
        public ProductGroup ParentProductGroup { get; set; }
        public List<ProductGroup> ChildProductGroups { get; set; }
    }
}