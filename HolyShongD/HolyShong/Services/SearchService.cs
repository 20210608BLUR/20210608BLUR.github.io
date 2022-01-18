using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using HolyShong.Models.HolyShongModel;
using HolyShong.Utility;

namespace HolyShong.Services
{


    public class SearchService
    {
        //初始相依性注入
        private readonly HolyShongRepository _repo;
        private readonly Calculate _calc;
        private readonly StoreService _storeService;       
        public SearchService()
        {
            _repo = new HolyShongRepository();
            _calc = new Calculate();
            _storeService = new StoreService();           
        }
        //StoreCategorySearch_依照主分類Id抓出所有店家
        public List<StoreCard> GetAllStoresByStoreCategoryId(int id)
        {
            var result = new List<StoreCard>();
            //找到特定的StoreCategory
            var storecategory = _repo.GetAll<StoreCategory>().FirstOrDefault(x => x.StoreCategoryId == id);
            if(storecategory==null)
            {
                return result;
            }
            //找到這個StoreCategory的所有Store
            var stores = _repo.GetAll<Store>().Where(x => x.StoreCategoryId == storecategory.StoreCategoryId);

            //將商店資訊存成卡片
            var cards = new List<StoreCard>();          
            foreach (var item in stores)
            {
                var card = new StoreCard()
                {
                    StoreId = item.StoreId,
                    StoreImg = item.Img,
                    StoreName = item.Name,
                    StoreScore = _storeService.GetStoreAverageScore(item.StoreId),
                    DiscountTag = _storeService.IsDiscountStore(item.StoreId)                  
                };
                cards.Add(card);
            }
            result = cards;
            return result;
        }

        //Search_搜尋頁面
        public List<StoreCard> GetAllStoresByRequest(SearchRequest request)
        {
            var result = new List<StoreCard>();
            //1.關鍵字
            var storesByKeyword = _storeService.GetAllStoresByKeyword(request.Keyword);

            //2.類別
            var storesByType = _storeService.GetAllStoresByType(request.Type, storesByKeyword);

            //3.價格範圍
            var storesByPrice = storesByType;
            if (!string.IsNullOrEmpty(request.Price))
            {
                storesByPrice = _storeService.GetAllStoresByPrice(request.Price, storesByPrice);
            }

            foreach (var item in storesByPrice)
            {
                var card = new StoreCard()
                {
                    StoreId = item.StoreId,
                    StoreImg = item.Img,
                    StoreName = item.Name,
                    StoreScore = _storeService.GetStoreAverageScore(item.StoreId),
                    DiscountTag = _storeService.IsDiscountStore(item.StoreId),
                };
                result.Add(card);
            }

            return result;
        }
    }
}