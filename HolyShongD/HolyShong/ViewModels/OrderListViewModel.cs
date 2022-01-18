using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        /// <summary>
        /// 訂單運送日期
        /// </summary>
        public DateTime DeliverDate { get; set; }
        /// <summary>
        /// 訂單狀況
        /// </summary>
        public int OrderStatus { get; set; } 

        public string RestaurantName { get; set; }
        public string RestaurantImg { get; set; }
        /// <summary>
        /// 訂單明細
        /// </summary>
        public List<OrderProduct> ProductLists { get; set; }
        public int ProductCount { get; set; }
        public decimal DeliverFee { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderProduct
    {
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string OrderOptions { get; set; }

    }
}