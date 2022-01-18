using HolyShong.Services;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HolyShong.Controllers
{
    public class StoreController : Controller
    {
        private readonly ProductService _productService;
        private readonly MarketService _marketService;
        public StoreController()
        {
            _productService = new ProductService();
            _marketService = new MarketService();
        }
        /// <summary>
        /// 餐廳頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Restaurant(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("NoSearch", "Home");
            }
            var loginMemberId = User.Identity.Name == "" ? 0 : int.Parse(User.Identity.Name);
            var result = _productService.GetStore(id.Value, loginMemberId);
            if (result.StoreName == null)
            {
                return RedirectToAction("NoSearch", "Home");
            }
            return View(result);



        }
        /// <summary>
        /// 新增喜愛店家 
        /// </summary>
        /// <returns></returns>
        public string CreateFavoriteStore(NowUrl nowUrl)
        {
            string storeId = "";
            int store = 0;
            var memberId = 0;
            //判斷是否登入
            var isLogin = int.TryParse(User.Identity.Name, out memberId);
            //如果沒登入,導到登入頁面
            if (isLogin == false)
            {
                return $"/Member/Login?url={nowUrl.UrlName}";

            }
            if (nowUrl.StoreId == 0)
            {
                string storeIdStr = System.Web.HttpContext.Current.Session["storeId"] == null ? storeId : System.Web.HttpContext.Current.Session["storeId"].ToString();
                store = int.Parse(storeIdStr);
            }
            //現在頁面
            else
            {
                store = nowUrl.StoreId;
            }
            _productService.FavoriteCreate(memberId, store);

            return "";
        }

        /// <summary>
        /// 刪除喜愛店家 
        /// </summary>
        /// <returns></returns>
        public string DeleteFavoriteStore(string storeId)
        {

            var memberId = 0;
            var isLogin = int.TryParse(User.Identity.Name, out memberId);
            //如果沒登入會員,按愛心,跳到登入會員畫面
            if (isLogin == false)
            {
                return "/Member/Login";
            }
            string storeIdStr = System.Web.HttpContext.Current.Session["storeId"] == null ? storeId : System.Web.HttpContext.Current.Session["storeId"].ToString();
            int storeIdInt = int.Parse(storeIdStr);

            _productService.FavoriteDelete(storeIdInt,memberId);

            return "";
        }

        /// <summary>
        /// 活動店家頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Marketing(int? id)
        {           
            if (!id.HasValue)
            {
                return RedirectToAction("NoSearch", "Home");
            }
            var loginMemberId = User.Identity.Name == "" ? 0 : int.Parse(User.Identity.Name);
            var result = _marketService.GetEventRestaurant(id.Value, loginMemberId);
            if (result.eventRestaurants == null)
            {
                return RedirectToAction("NoSearch", "Home");
            }
            return View(result);
        }

        /// <summary>
        /// 是否為喜愛店家
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public bool ReadMemberHeart(string storeId)
        {
            string storeIdStr = System.Web.HttpContext.Current.Session["storeId"] == null ? storeId : System.Web.HttpContext.Current.Session["storeId"].ToString();
            int storeIdInt = int.Parse(storeIdStr);
            int memberId = int.Parse(User.Identity.Name);
            bool result = _productService.ReadHeart(storeIdInt, memberId);
            return result;
        }
    }
}