using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Store
{
    public class CreateStoreViewModel
    {
        public StoreInfoViewModel StoreInfo { get; set; }
        public TimeInfoViewModel TimeInfo { get; set; }
        public string[] CategoryInfo { get; set; }
        public ProductInfoViewModel[] ProductInfo { get; set; }

    }
    public class StoreInfoViewModel
    {
        //店家名稱
        public string Name { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public string ChargeName { get; set; }
        public string ChargePhone { get; set; }
        public string Img { get; set; }
        public string[] Keyword { get; set; }

        public string Phone { get; set; }
    }
    public class TimeInfoViewModel
    {
        public object Monday { get; set; }
        public object Tuesday { get; set; }
        public object Wednesday { get; set; }
        public object Thursday { get; set; }
        public object Friday { get; set; }
        public object Saturday { get; set; }
        public object Sunday { get; set; }

    }

    public class ProductInfoViewModel
    {
        public OptionViewModel[] OptionList { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImg { get; set; }
        public string ProductName { get; set; }
        public string UnitPrice { get; set; }
    }

    public class OptionViewModel
    {
        public string ProductOptionName { get; set; }
        public string ProductOptionDetailName { get; set; }
        public string AddPrice { get; set; }

    }
}
