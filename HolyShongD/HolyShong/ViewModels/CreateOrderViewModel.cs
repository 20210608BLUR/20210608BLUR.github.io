using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class CreateOrderViewModel
    {
        //消費者外送註記
        public string N { get; set; }
        //需要餐具
        public string T { get; set; }
        //需要塑膠袋
        public string P { get; set; }
        //會員優惠Id
        public int M { get; set; }
        //外送費
        public decimal D { get; set; }


    }
}