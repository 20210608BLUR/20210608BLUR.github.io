using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class OrderStatusViewModel
    {
        public int MemberId { get; set; }
        public string OrderCode { get; set; }
        public int OrderStatus { get; set; }
    }
}