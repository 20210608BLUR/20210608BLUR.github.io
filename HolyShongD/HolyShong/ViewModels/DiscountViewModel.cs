using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class DiscountViewModel
    {
        public string DiscountName { get; set; }
        public DateTime EndTime { get; set; }
        public int DiscountMemberId { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class DiscountCodeViewModel
    {
        public string DiscountCode { get; set; } 
    }
}