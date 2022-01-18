using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Store
{
    public class CreateProductViewModel
    {
        public string StoreName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductImg { get; set; }
        public decimal UnitPrice { get; set; }
        public List<CreateOptionViewModel> OptionList { get; set; }
    }
    public class CreateOptionViewModel
    {
        public string ProductOptionName { get; set; }
        public string ProductOptionDetailName { get; set; }
        public decimal AddPrice { get; set; }
    }
}
