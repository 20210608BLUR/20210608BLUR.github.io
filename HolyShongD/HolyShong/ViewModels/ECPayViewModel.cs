using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    /// <summary>
    /// 傳入ECPay的參數
    /// </summary>
    public class ECPayViewModel
    {
        //會員Id，帶資料用不顯示
        public int MemberId { get; set; }
        public int OrderId { get; set; }
        public string ReturnURL { get; set; }
        //瀏覽器端返回的廠商網址
        public string ClientBackURL { get; set; }
        //瀏覽器端回傳付款結果網址
        public string OrderResultURL { get; set; }
        //廠商的交易編號
        public string MerchantTradeNo { get; set; }
        //交易總金額
        public decimal TotalAmount { get; set; }
        //購買商品明細
        public List<BuyItem> ListItems { get; set; }
        public string CheckOutInfo { get; set; }
    }

    /// <summary>
    /// 購買商品明細
    /// </summary>
    public class BuyItem
    {
        //產品名稱
        public string Name { get; set; }
        //商品單價
        public decimal Price { get; set; }
        //幣別單位，固定為新台幣
        public string Currency { get; set; }
        //購買數量
        public int Quantity { get; set; }
        //商品說明網址，固定為空值
        public string URL { get; set; }
    }

    /// <summary>
    /// 交易後，綠界傳來的資料
    /// </summary>
    public class GetResultFromECPay
    {
        //特店編號
        public string MerchantID { get; set; }
        //特店交易編號
        public string MerchantTradeNo { get; set; }
        //特店旗下店舖代號
        public string StoreID { get; set; }
        //交易狀態
        public int RtnCode { get; set; }
        //交易訊息
        public string RtnMsg { get; set; }
        //綠界交易編號
        public string TradeNo { get; set; }
        //交易金額
        public int TradeAmt { get; set; }
        //付款時間
        public string PaymentDate { get; set; }
        //特店選擇的付款方式
        public string PaymentType { get; set; }
        //交易手續費
        public int PaymentTypeChargeFee { get; set; }
        //訂單成立時間
        public string TradeDate { get; set; }
        //是否為模擬付款
        public int SimulatePaid { get; set; }
        //自訂名稱欄位1=>存會員id
        public string CustomField1 { get; set; }
        //自訂名稱欄位2=>存訂單編號
        public string CustomField2 { get; set; }
        //自訂名稱欄位3
        public string CustomField3 { get; set; }
        //自訂名稱欄位4
        public string CustomField4 { get; set; }
        //檢查碼
        public string CheckMacValue { get; set; }
    }
       
}