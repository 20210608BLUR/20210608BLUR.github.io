using HolyShong.Models.HolyShongModel;
using HolyShong.Services;
using HolyShong.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HolyShong.WebAPI
{

    public class CartController : ApiController
    {
        private readonly CartService _cartService;
        public CartController()
        {
            _cartService = new CartService();
        }
        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="storeProductVM"></param>
        /// <returns></returns>
        [HttpPost]
        public OperationResult AddToCart(StoreProduct storeProductVM)
        {
            OperationResult result = new OperationResult();
            List<StoreProduct> cartList = new List<StoreProduct>();
            var session = HttpContext.Current.Session; //宣告Session
            Cart cart = new Cart();
            try
            {
                int memberId = 0;
                //判斷是否有登入
                bool isLogin = int.TryParse(User.Identity.Name, out memberId);
                //未登入加入session
                if (isLogin == false)
                {
                    //沒有購物車
                    if (session["Cart"] == null)
                    {
                        cartList.Add(storeProductVM);
                        session["Cart"] = cartList;
                    }
                    //有購物車
                    else
                    {
                        var cuurentList = (List<StoreProduct>)session["Cart"];

                        //判斷加入商品店家是否相同
                        var isStoreMatch = _cartService.MatchStoreNoLogin(cuurentList, storeProductVM);
                        //店家不同
                        if (!isStoreMatch)
                        {
                            //刪除舊的購物車，加入新的購物車
                            session.Remove("Cart");
                        }
                        cartList.Add(storeProductVM);
                        session["Cart"] = cartList;

                    }
                }
                //有登入
                else
                {
                    //判斷是否有無購物車，有讀出並加入，沒有的話，直接加入
                    var isCart = _cartService.SearchCart(memberId);
                    //有購物車
                    if(isCart != null)
                    {
                        cartList.Add(storeProductVM);
                        //比對店家
                        var isStoreMatch = _cartService.MatchCartStore(cartList, memberId);
                        //店家不同
                        if (isStoreMatch == false)
                        {
                            //刪除舊的購物車，加入新的購物車
                            cart = _cartService.ChangeCart(cartList, memberId);
                            cartList = _cartService.CurrentCart(cart);
                        }
                        //店家相同，直接加入
                        else
                        {
                            cart = _cartService.AddProductToCart(storeProductVM, memberId);
                            cartList = _cartService.CurrentCart(cart);
                        }
                    }
                    //沒購物車
                    else
                    {
                        cart = _cartService.AddProductToCart(storeProductVM, memberId);
                        cartList = _cartService.CurrentCart(cart);
                    }
                }
                result.IsSuccessful = true;
                result.Exception = String.Empty;
                result.Result = cartList;
            }
            catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.Exception = ex.ToString();
                result.Result = null;
            }
            return result;
        }

        /// <summary>
        /// 呈現購物車
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public OperationResult ShowCart()
        {
            OperationResult result = new OperationResult();
            List<StoreProduct> cartList = new List<StoreProduct>();

            try
            {
                int memberId = 0;
                //判斷是否有登入
                bool isLogin = int.TryParse(User.Identity.Name, out memberId);
                //尚未登入Session讀出
                if (!isLogin)
                {
                    var session = HttpContext.Current.Session;
                    cartList = (List<StoreProduct>)session["Cart"] == null ? new List<StoreProduct>() : (List<StoreProduct>)session["Cart"];
                }
                //已登入從資料庫讀出
                else
                {
                    var findCart = _cartService.SearchCart(memberId);
                    //有購物車
                    if(findCart != null)
                    {
                        cartList = _cartService.CurrentCart(findCart);
                    }
                }
                result.IsSuccessful = true;
                result.Exception = String.Empty;
                result.Result = cartList;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Exception = ex.ToString();
                result.Result = null;
            }
            return result;
        }

        /// <summary>
        /// 修改購物車內容(刪除、數量改變)
        /// </summary>
        /// <param name="storeProductVMs"></param>
        /// <returns></returns>
        [HttpPost]
        public OperationResult ChangeCartItem(List<StoreProduct> storeProductVMs)
        {
            OperationResult result = new OperationResult();
            List<StoreProduct> cartList = new List<StoreProduct>();
            var session = HttpContext.Current.Session;

            try
            {
                int memberId = 0;
                var isLogin = int.TryParse(User.Identity.Name, out memberId);
                //沒登入
                if(!isLogin)
                {
                    session.Remove("Cart");
                    session["Cart"] = storeProductVMs;
                    cartList = storeProductVMs;
                }
                //有登入
                else
                {
                    var cart = _cartService.ChangeCart(storeProductVMs, memberId);
                    //VM
                    cartList = _cartService.CurrentCart(cart);
                }
                result.IsSuccessful = true;
                result.Exception = String.Empty;
                result.Result = cartList;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Exception = ex.ToString();
                result.Result = null;
            }
            return result;
        }
    }
}
