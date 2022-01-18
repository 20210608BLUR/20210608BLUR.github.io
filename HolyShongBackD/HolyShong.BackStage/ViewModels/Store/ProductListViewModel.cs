using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Store
{
    public class ProductListViewModel
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductImg { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsEnable { get; set; }

        public List<ProductOptionViewModel> OptionList { get; set; }
    }
    public class ProductOptionViewModel
    {
        public int ProductOptionId { get; set; }
        public string ProductOptionName { get; set; }
        public List<ProductOptionDetailViewModel> OptionDetailList { get; set; }
    }
    public class ProductOptionDetailViewModel
    {
       public int ProductOptionId { get; set; }
        public string ProductOptionName { get; set; }
        public int ProductOptionDetailId { get; set; }
        public string ProductOptionDetailName { get; set; }
        public decimal AddPrice { get; set; }
    }

    //API
    public class ProductStatusViewModel
    {
        public bool IsOnSale { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Keyword { get; set; }
    }

    public class UpdateProductViewModel
    {
        public int ID { get; set; }
        public bool SaleStatus { get; set; }
    }

    public class ProductInfo
    {
        public int ProductId { get; set; }
    }

    public class SearchStore
    {
        public string StoreName { get; set; }
    }

    public class SearchStoreInfo
    {
        public string StoreName { get; set; }
        public string[] ProductCateArray { get; set; }
        public string ResponseMsg { get; set; }
    }


}
