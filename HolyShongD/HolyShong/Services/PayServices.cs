using ECPay.Payment.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.ViewModels;
using HolyShong.Models.HolyShongModel;
using System.Data.Entity;
using HolyShong.Repositories;
using Newtonsoft.Json;
using Item = HolyShong.Models.HolyShongModel.Item;

namespace HolyShong.Services
{
    public class PayServices
    {
        private readonly HolyShongRepository _repo;

        public PayServices()
        {
            _repo = new HolyShongRepository();
        }

        //設定回傳綠界api路徑
        string VIPReturnURL = "https://holyshong-frontstage.azurewebsites.net/api/payment/VIPGetResultFromECPay";
        string BuyCartReturnURL = "https://holyshong-frontstage.azurewebsites.net/api/payment/BuyCartGetResultFromECPay";
        //string feeReturnURL = "https://holyshongtest.azurewebsites.net/api/payment/VIPGetResultFromECPay";

        string VIPClientBackURL = "https://holyshong-frontstage.azurewebsites.net/Member/UserProfile";
        string BUYClientBackURL = "https://holyshong-frontstage.azurewebsites.net/Order/Index";

        /// <summary>
        /// 綠界SDK
        /// </summary>
        /// <param name="eCPayViewModel"></param>
        /// <returns></returns>
        public string ECPayBasic(ECPayViewModel eCPayViewModel)
        {
            AllInOne oPayment = new AllInOne();
            /* 服務參數 */
            oPayment.ServiceMethod = HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法            
            oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
            oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
            oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
            oPayment.MerchantID = "2000132";//ECPay提供的特店編號

            /* 基本參數 */
            oPayment.Send.ReturnURL = eCPayViewModel.ReturnURL;//付款完成通知回傳的網址，測試版隨ngrok調整
            oPayment.Send.ClientBackURL = eCPayViewModel.ClientBackURL;//瀏覽器端返回的廠商網址
            oPayment.Send.OrderResultURL = "";//瀏覽器端回傳付款結果網址
            oPayment.Send.MerchantTradeNo = "HolyShong" + new Random().Next(0, 99999).ToString();//廠商的交易編號
            oPayment.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
            oPayment.Send.TotalAmount = eCPayViewModel.TotalAmount;//交易總金額
            oPayment.Send.TradeDesc = "交易描述";//交易描述
            oPayment.Send.ChoosePayment = PaymentMethod.Credit;//使用的付款方式
            oPayment.Send.Remark = "";//備註欄位
            oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
            oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.No;//是否需要額外的付款資訊
            oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
            oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
            oPayment.Send.PlatformID = "";//特約合作平台商代號
            oPayment.Send.HoldTradeAMT = HoldTradeType.Yes;
            oPayment.Send.CustomField1 = eCPayViewModel.MemberId.ToString();//備註欄，會員Id
            oPayment.Send.CustomField2 = eCPayViewModel.CheckOutInfo;
            oPayment.Send.CustomField3 = "";
            oPayment.Send.CustomField4 = "";
            oPayment.Send.EncryptType = 1;

            //訂單的商品資料
            var buyitems = eCPayViewModel.ListItems;
            foreach (var buyitem in buyitems)
            {
                oPayment.Send.Items.Add(new ECPay.Payment.Integration.Item()
                {
                    Name = buyitem.Name,//商品名稱
                    Price = buyitem.Price,//商品單價
                    Currency = "新台幣",//固定幣別單位
                    Quantity = buyitem.Quantity,//購買數量
                    URL = "",//商品的說明網址
                });
            }

            var temp = JsonConvert.SerializeObject(oPayment.Send);

            var html = string.Empty;
            oPayment.CheckOutString(ref html);
            return html;
        }

        internal object BuyCartService(int memberId, CheckOutViewModel checkoutVM, decimal totalPrice)
        {
            int price =(int)totalPrice;
            //var OrderForPay = _repo.GetAll<Order>().Where(x => x.OrderStatus == 0 && x.PaymentStatus == 0).First(x => x.MemberId == memberId);
            var createOrderVM = new CreateOrderViewModel()
            {
                D = checkoutVM.DeliverFee,
                M = checkoutVM.MemberDiscountId,
                P = checkoutVM.IsPlasticbag,
                T = checkoutVM.IsTablewares,
                N = checkoutVM.CustomerNote
            };

            var createOrderJson = JsonConvert.SerializeObject(createOrderVM);
            var createOrderReplace = createOrderJson.Replace("\"","?");

            var OrderDetailOptaionForPay = new ECPayViewModel()
            {
                MemberId = memberId,
                //Todo 改連結=>綠界返回商店的連結
                ClientBackURL = BUYClientBackURL,
                ReturnURL = BuyCartReturnURL,
                OrderResultURL = "",
                MerchantTradeNo = "",
                CheckOutInfo = createOrderReplace,
                TotalAmount = price,
                //TotalAmount = 150,
                ListItems = new List<BuyItem>
                {
                    new BuyItem
                    {
                        Name = checkoutVM.StoreName,
                        Currency = "新臺幣",
                        Price = totalPrice,
                        Quantity = 1,
                        URL = "",
                    }
                },
            };

            string html = ECPayBasic(OrderDetailOptaionForPay);
            return html;
        }

        //todo

        /// <summary>
        /// 升級送送會員
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BecomeVIPService(int id)
        {

            var becomVIPObject = new ECPayViewModel()
            {
                MemberId = id,
                ClientBackURL = VIPClientBackURL,//瀏覽器端返回的廠商網址
                ReturnURL = VIPReturnURL,
                OrderResultURL = "",//瀏覽器端回傳付款結果網址
                MerchantTradeNo = "",//廠商的交易編號
                TotalAmount = 120,//交易總金額
                ListItems = new List<BuyItem>
                {
                  new BuyItem
                  {
                      Name="升級送送會員",
                      Currency="新臺幣",
                      Price=120,
                      Quantity=1,
                      URL="",
                  }
                },
            };

            string html = ECPayBasic(becomVIPObject);
            return html;
        }

        /// <summary>
        /// 送送會員付款成功
        /// </summary>
        /// <param name="id"></param>
        public void BecomVIPSuccess(int id)
        {
            //製作Rank欄位
            var rank = new Rank()
            {
                MemberId = id,
                IsPrimary = true,
                EndTime = DateTime.UtcNow.AddHours(8).AddDays(30),
            };

            using (var transaction = _repo.Context.Database.BeginTransaction())
                try
                {
                    _repo.Create<Rank>(rank);
                    _repo.SaveChange();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
        }



        //Todo 不能再從資料庫抓購物車資料
        /// <summary>
        /// 購買購物車內商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public string BuyCartService(int id, CheckOutViewModel checkoutVM)
        //{
        //    var OrderForPay = _repo.GetAll<Order>().Where(x => x.OrderStatus == 0 && x.PaymentStatus == 0).First(x => x.MemberId == id);


        //    var OrderDetailOptaionForPay = new ECPayViewModel()
        //    {
        //        MemberId = id,
        //        //Todo 改連結=>綠界返回商店的連結
        //        ClientBackURL = "https://holyshongtest.azurewebsites.net/Member/OrderList",
        //        ReturnURL = BuyCartReturnURL,
        //        OrderResultURL = "",
        //        MerchantTradeNo = "",
        //        TotalAmount = checkoutVM.TotalPrice,
        //        ListItems = new List<BuyItem>
        //        {
        //            new BuyItem
        //            {
        //                Name = checkoutVM.StoreName,
        //                Currency = "新臺幣",
        //                Price = checkoutVM.TotalPrice,
        //                Quantity = 1,
        //                URL = "",
        //            }
        //        },


        //    };

        //    string html = ECPayBasic(OrderDetailOptaionForPay);
        //    return html;
        //}
    }

}