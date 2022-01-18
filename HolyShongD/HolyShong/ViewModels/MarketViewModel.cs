using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class MarketViewModel
    {
        /// <summary>
        /// 活動title
        /// </summary>
        public string EventTitle { get; set; }
        /// <summary>
        /// 活動內容
        /// </summary>
        public string EventContent { get; set; }
        /// <summary>
        /// 活動開始
        /// </summary>
        public DateTime EventStart { get; set; }
        /// <summary>
        /// 活動結束
        /// </summary>
        public DateTime EventEnd { get; set; }
        /// <summary>
        /// 很多活動店家
        /// </summary>
        public List<EventRestaurant> eventRestaurants { get; set; }
        
    }
    public class EventRestaurant
    {
        
        public int StoreId { get; set; }
        /// <summary>
        /// 店家圖片
        /// </summary>
        public string RestaurantImg { get; set; }
        /// <summary>
        /// 店家名稱
        /// </summary>
        public string RestaurantName { get; set; }
        /// <summary>
        /// 運費
        /// </summary>
        public decimal DeliveryFee { get; set; }
        /// <summary>
        /// 評分
        /// </summary>
        public int RestaurantScore { get; set; }
        /// <summary>
        /// 是否為喜愛店家
        /// </summary>
        public bool isFavStore { get; set; }
        public bool DiscountTag { get; set; }
        public decimal StoreScore { get; set; }


    }
   
}