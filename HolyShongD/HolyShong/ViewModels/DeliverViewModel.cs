using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class DeliverViewModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderCode { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustormerNotes { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int OrderStatus { get; set; }
    }

    public class OrderProducts
    {
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
    }

}