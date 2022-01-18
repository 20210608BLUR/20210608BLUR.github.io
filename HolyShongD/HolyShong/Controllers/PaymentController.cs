using ECPay.Payment.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HolyShong.Services;
using HolyShong.ViewModels;

namespace HolyShong.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PayServices _payServices;
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly DiscountService _discountService;

        public PaymentController()
        {
            _payServices = new PayServices();
            _orderService = new OrderService();
            _cartService = new CartService();
            _discountService = new DiscountService();
        }

        // GET: ECPay
        [Authorize]
        public ActionResult BecomeVIP()
        {          
            var html = _payServices.BecomeVIPService(int.Parse(User.Identity.Name));
            ViewBag.Html = html;
            return View();
        }

        [Authorize]
        public ActionResult PayForCart(CheckOutViewModel checkoutVM)
        {
            var memberId = int.Parse(User.Identity.Name);
            //撈資料庫cart
            var findCart = _cartService.SearchCart(memberId);
            //購物車金額計算
            var cartPrice = _cartService.CalculateMoneyByCart(findCart);
            //優惠後金額計算
            var discountPrice = _discountService.CalculateTotalWithDiscountMemberId(checkoutVM.MemberDiscountId, cartPrice);
            //總額計算
            var totalPrice = discountPrice + checkoutVM.DeliverFee;
            totalPrice = (int)totalPrice;

            var html = _payServices.BuyCartService(memberId,checkoutVM,(int)totalPrice);
            ViewBag.Html = html;
            return View();
        }
    }
}