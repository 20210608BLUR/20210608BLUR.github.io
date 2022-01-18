using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.EnumUtility
{
    public  static class StatusEnum
    {
        public enum OrderStatus
        {
            未付款,
            已付款,
            餐點完成,
            等待配送,
            配送中,
            已配送
        }

        public enum DeliverStatus
        {
            未配送 = 1,
            配送中 =2,
            已配送 =3
        }

        public enum PaymentStatus
        {
            已付款 = 1,
            已退款 = 2
        }

        public enum WeekDay
        {
            星期天,
            星期一,
            星期二,
            星期三,
            星期四,
            星期五,
            星期六
        }

    }
}