using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Order
{
    public class OrderResponseViewModel
    {
        /// <summary>
        /// 訂單ID
        /// 呈現
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 會員ID
        /// 呈現
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 外送員ID
        /// </summary>
        public int? DeliverId { get; set; }

        /// <summary>
        /// 消費店家ID
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 消費店家名稱
        /// 呈現
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal DeliverFee { get; set; }

        /// <summary>
        /// 小費
        /// </summary>
        public decimal? Tips { get; set; }

        /// <summary>
        /// 訂單備註
        /// </summary>
        public string Note { get; set; }
        
        /// <summary>
        /// 送達地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 要不要餐具
        /// </summary>
        public bool IstableWares { get; set; }

        /// <summary>
        /// 要不要塑膠袋
        /// </summary>
        public bool IsPlasticbag { get; set; }

        /// <summary>
        /// 金流狀態
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 外送狀態
        /// 呈現
        /// </summary>
        public int DeliverStatus { get; set; }

        /// <summary>
        /// 訂單狀態
        /// 呈現
        /// 可編輯
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 創立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 送達時間
        /// </summary>
        public DateTime RequiredDate { get; set; }

        /// <summary>
        /// 優惠Id
        /// </summary>
        public int? MemberDiscountId { get; set; }

        /// <summary>
        /// 優惠名稱
        /// 呈現
        /// </summary>
        public string DiscountName { get; set; }

        /// <summary>
        /// 評分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 訂單金額(含產品金額、運費；不含小費與折扣)
        /// 呈現
        /// </summary>
        public decimal OrderPrice { get; set; }

        /// <summary>
        /// 折扣金額
        /// </summary>
        public decimal? DiscountMoney { get; set; }

        /// <summary>
        /// 訂單狀態中文
        /// </summary>
        public string OrderStatusName { get; set; }

        /// <summary>
        /// 是否可以修改資料的按鈕
        /// 預設為true為不可修改
        /// </summary>
        public bool NotCheck { get; set; }

        public IEnumerable<OrderDetailResponse> OrderDetailResponses { get; set; }
    }

    public class OrderDetailResponse
    {
        /// <summary>
        /// 訂單明細ID
        /// </summary>
        public int OrderDetailId { get; set; }

        /// <summary>
        /// 訂單ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 產品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 產品名稱        
        /// 關聯Product資料表
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 產品單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 購買數量
        /// </summary>
        public int Quantity { get; set; }        

        public IEnumerable<OrderDetailOptionResponse> OrderDetailOptionResponses { get; set; }
    }

    public class OrderDetailOptionResponse
    {
        /// <summary>
        /// 訂單明細選項Id
        /// </summary>
        public int OrderOptionIdDetailId { get; set; }

        /// <summary>
        /// 訂單明細Id
        /// </summary>
        public int OrderDeetailId { get; set; }



        /// <summary>
        /// 產品明細Id
        /// </summary>
        public decimal? ProductOptionDetailId { get; set; }

        /// <summary>
        /// 產品明細選項說明
        /// </summary>
        public string ProductOptionDetailName { get; set; }

        /// <summary>
        /// 產品明細選項加錢
        /// </summary>
        public decimal? Addprice { get; set; }

    }
}

