using HolyShong.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HolyShong.Models.HolyShongModel;
using HolyShong.ViewModels;
using HolyShong.Utility;

namespace HolyShong.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;
        private readonly StoreService _storeService;
        private readonly SearchService _searchService;
        private readonly ProductService _productService;
        public HomeController()
        {
            _homeService = new HomeService();
            _storeService = new StoreService();
            _searchService = new SearchService();
            _productService = new ProductService();
        }
        //初始

        public ActionResult Index(int ? id)//首頁
        {
            if (!id.HasValue)
            {

            }
            var loginMemberId = User.Identity.Name == "" ? 0 : int.Parse(User.Identity.Name);
            var result = new HomeViewModel()
            {
                StoreCategoryBlocks = new List<HomeStoreCategory>(),
                DiscountBlocks = new List<HomeDiscount>(),
                FavorateBlocks = new List<StoreCardBlock>(),
            };

            result.StoreCategoryBlocks = _homeService.GetAllStoreCategories();
            result.DiscountBlocks = _homeService.GetAllDiscount();
            result.FavorateBlocks = _homeService.GetAllFavoriteByMemberId(loginMemberId);
            result.StoreCardBlocks = _homeService.GetRandomStores(loginMemberId);
            //ViewData["memberId"] = loginMemberId;
            return View(result);
        }

        public ActionResult StoreCategorySearch(int id)//主分類搜尋頁
        {
            var result = new StoreCategorySearchViewModel()
            {
                StoreCards = new List<StoreCard>(),
            };
            //if (string.IsNullOrEmpty(name))
            //{
            //    return RedirectToAction("NoSearch");
            //}
            //result.StoreCards = _searchService.GetAllStoresByStoreCategoryId(id);
            //if (result.StoreCards.Count() == 0)
            //{
            //    return RedirectToAction("NoSearch");
            //}
            //if (!id.HasValue)
            //{
            //    return RedirectToAction("NoSearch");
            //}
            result.StoreCards = _searchService.GetAllStoresByStoreCategoryId(id);
            return View(result);
        }

        public ActionResult NoSearch()//搜尋不到頁面
        {
            return View();
        }


        public ActionResult Search(SearchRequest request)//搜尋頁面
        {
            request.Keyword = string.IsNullOrEmpty(request.Keyword) ? string.Empty : request.Keyword;
            request.Price = string.IsNullOrEmpty(request.Price) ? string.Empty : request.Price;


            var result = new SearchViewModel()
            {
                StoreCards = _searchService.GetAllStoresByRequest(request),
            };

            //轉換其它頁面
            if (result.StoreCards.Count() == 0)
            {
                return View("NoSearch");
            }
            request.SearchCount = result.StoreCards.Count();
            ViewBag.searchTemp = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            return View(result);
        }

        /// <summary>
        /// 是否為喜愛店家
        /// </summary>
        /// <returns></returns>
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