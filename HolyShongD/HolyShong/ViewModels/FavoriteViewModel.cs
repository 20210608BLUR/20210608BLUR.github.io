using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class FavoriteViewModel
    {       
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最愛店家卡片
        /// </summary>
        public List<FavoriteStore> favoriteStores { get; set; }
    }
    public class FavoriteStore
    {
        public int StoreId { get; set; }        
        public string StoreImg { get; set; }
        public string StoreName { get; set; }
        public bool DiscountTag { get; set; }
        public decimal StoreScore { get; set; }
    }

    /// <summary>
    /// 登入後,導到上一頁
    /// </summary>
    public class NowUrl
    {
        public string UrlName { get; set; }
        public int StoreId { get; set; }
    }
}