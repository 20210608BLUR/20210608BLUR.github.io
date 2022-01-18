using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class HomeViewModel
    {
        public int MemberId { get; set; }
        public List<HomeStoreCategory> StoreCategoryBlocks { get; set; }
        public List<HomeDiscount> DiscountBlocks { get; set; }
        public List<StoreCardBlock> FavorateBlocks { get; set; }
        public List<StoreCardBlock> StoreCardBlocks { get; set; }
    }
    //主分類區
    public class HomeStoreCategory
    {
        public int StoreCategoryId { get; set; }
        public string StoreCategoryImg { get; set; }
        public string StoreCategoryName { get; set; }
    }
    //活動店家區
    public class HomeDiscount
    {
        public int DiscountStoreId { get; set; }
        public string DiscountStoreImg { get; set; }
    }
    //主頁面區
    public class StoreCardBlock
    {
        public int StoreCategoryId { get; set; }
        public string StoreCategoryName { get; set; }
        //主頁面區
        public List<StoreCard> StoreCards { get; set; }
    }
    //卡片元素
    public class StoreCard
    {
        public int StoreId { get; set; }
        public string StoreImg { get; set; }
        public string StoreName { get; set; }
        public decimal StoreScore { get; set; }
        public bool DiscountTag { get; set; }
        public string StoreAveragePrice { get; set; }
        public string DeliverTime { get; set; }
        /// <summary>
        /// 是否為喜愛店家
        /// </summary>
        public bool isFavStore { get; set; }

    }
}