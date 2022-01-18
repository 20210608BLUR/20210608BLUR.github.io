using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class CheckOutViewModel
    {
        public string StoreName { get; set; }
        public string CustomerNote { get; set; }
        public string IsTablewares { get; set; }
        public string IsPlasticbag { get; set; }
        public int MemberDiscountId  { get; set; }
        public decimal DeliverFee { get; set; }
        public decimal TotalPrice { get; set; }
    }
}