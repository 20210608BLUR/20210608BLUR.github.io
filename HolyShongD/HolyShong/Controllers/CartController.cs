using HolyShong.Models.HolyShongModel;
using HolyShong.Services;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HolyShong.Controllers
{
    public class CartController : Controller
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        public CartController()
        {
            _orderService = new OrderService();
            _cartService = new CartService();
        }

        /// <summary>
        /// 結帳頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckOut()
        {
            List<StoreProduct> cartList = new List<StoreProduct>();
            //判斷是否登入
            var memberId = 0;
            var isLogin = int.TryParse(User.Identity.Name, out memberId);
            if (isLogin == false)
            {
                return RedirectToAction("Login", "Member");
            }
            else
            {
                //登入直接導向結帳頁面
                var loginCart = _cartService.SearchCart(memberId);
                cartList = _cartService.CurrentCart(loginCart);
            }
            var result = _cartService.ToCheckOut(cartList, memberId);
            return View(result);
        }
    }
}