using System.Collections.Generic;

namespace HolyShong.ViewModels
{
    public class ProductViewModel
    {
        /// <summary>
        /// 商店Id
        /// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// 商店名稱
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 評分
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 商店圖片
        /// </summary>
        public string StoreImg { get; set; }
        /// <summary>
        /// 商店分類名稱
        /// </summary>
        public string StoreCategoryName { get; set; }
        /// <summary>
        /// 商店地址
        /// </summary>
        public string StoreAddress { get; set; }
        /// <summary>
        /// 很多商品分類
        /// </summary>
        public List<StoreProductCategory> StoreProductCategories { get; set; }
        public string SupplyTimes { get; set; }
        /// <summary>
        /// 是否為喜愛店家
        /// </summary>
        public bool isFavStore { get; set; }
    }
    //public class SupplyTime
    //{
    //    public int WeekDay { get; set; }
    //    public DateTime OpenTime { get; set; }
    //    public DateTime CloseTime { get; set; }
    //}
    public class StoreProductCategory
    {
        /// <summary>
        /// 商品分類名稱
        /// </summary>
        public string StoreProductCategoryName { get; set; }

        /// <summary>
        /// 分類順序
        /// </summary>
        public int StoreProductCategorySort { get; set; }
        /// <summary>
        /// 很多商品
        /// </summary>
        public List<StoreProduct> StoreProducts { get; set; }
    }

    public class StoreProduct
    {
        public string StoreName { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品敘述
        /// </summary>
        public string ProductDescription { get; set; }
        /// <summary>
        /// 商品金額
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 商品圖片
        /// </summary>
        public string ProductImg { get; set; }
        /// <summary>
        /// 很多商品option
        /// </summary>
        public List<StoreProductOption> StoreProductOptions { get; set; }

        public int Quantity { get; set; }
        
    }
    public class StoreProductOption
    {
        public int SelectOptionPrice { get; set; }
        public string SelectOption { get; set; }
        /// <summary>
        /// 商品option名稱(辣度)
        /// </summary>
        public string ProductOptionName { get; set; }
        /// <summary>
        /// 很多商品optionDetail(大辣)
        /// </summary>
        public List<StoreProductOptionDetail> ProductOptionDetails { get; set; }
    }
    public class StoreProductOptionDetail
    {
        /// <summary>
        /// 商品optionDetail名稱
        /// </summary>
        public string StoreProductOptioinDetailName { get; set; }
        public int StoreProductOptionDetailId { get; set; }
        public decimal AddPrice { get; set; }
    }
}