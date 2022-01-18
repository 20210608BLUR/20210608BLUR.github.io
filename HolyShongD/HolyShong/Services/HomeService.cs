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
    public class HomeService
    {
        //初始相依性注入
        private readonly HolyShongRepository _repo;
        private readonly Calculate _calc;
        private readonly StoreService _storeService;
        private readonly ProductService _productService;
        public HomeService()
        {
            _repo = new HolyShongRepository();
            _calc = new Calculate();
            _storeService = new StoreService();
            _productService = new ProductService();
        }

        //主分類提取
        public List<HomeStoreCategory> GetAllStoreCategories()
        {
            //初始化
            var result = new List<HomeStoreCategory>();

            //取出所有主分類
            var storecategories = _repo.GetAll<StoreCategory>();

            //存取所有主分類
            foreach (var item in storecategories)
            {
                var temp = new HomeStoreCategory();
                temp.StoreCategoryId = item.StoreCategoryId;
                temp.StoreCategoryImg = item.Img;
                temp.StoreCategoryName = item.Name;
                result.Add(temp);
            }
            return result;
        }

        //活動區塊提取
        public List<HomeDiscount> GetAllDiscount()
        {
            //初始化
            var result = new List<HomeDiscount>();

            //取出所有活動頁
            var discounts = _repo.GetAll<Discount>();

            //存取所有活動頁
            foreach (var item in discounts)
            {
                var temp = new HomeDiscount();
                temp.DiscountStoreId = item.DiscountId;
                temp.DiscountStoreImg = item.Img;
                result.Add(temp);
            }
            return result;
        }

        //依照會員Id最愛店家區塊提取
        public List<StoreCardBlock> GetAllFavoriteByMemberId(int memberId)
        {
            //初始化
            var result = new List<StoreCardBlock>();

            //依據會員找出所有最愛店家並且排序
            var favorites = _repo.GetAll<Favorite>().Where(f => f.MemberId == memberId).OrderByDescending(f => f.CreateTime);

            //宣告新參數存取店家資訊
            var stores = _repo.GetAll<Store>()
                .Where(s => favorites.Select(f => f.StoreId).Contains(s.StoreId)).ToList()
                .Select(s => new
                {
                    StoreId = s.StoreId,
                    Img = s.Img,
                    Name = s.Name,
                    Index = favorites.ToList().FindIndex(f => f.StoreId == s.StoreId)
                }).OrderBy(s => s.Index);

            //將商店資訊存成卡片
            var Cards = new List<StoreCard>();
          
            foreach (var item in stores)
            {
                var card = new StoreCard
                {
                    StoreId = item.StoreId,
                    StoreImg = item.Img,
                    StoreName = item.Name,
                    StoreScore = _storeService.GetStoreAverageScore(item.StoreId),
                    DiscountTag = _storeService.IsDiscountStore(item.StoreId),
                    isFavStore = _productService.ReadHeart(item.StoreId, memberId)
                };
                Cards.Add(card);
            }

            //將卡片加進區塊
            var block = new StoreCardBlock
            {
                StoreCards = Cards
            };
            result.Add(block);
            return result;
        }

        //隨機取出五種類別並隨機取出五間裡面的店家
        public List<StoreCardBlock> GetRandomStores(int memberId)
        {
            //初始化
            var result = new List<StoreCardBlock>();

            //隨機取出5種商店主分類Id
            var storecategoryIdList = _repo.GetAll<StoreCategory>().Select(sc => sc.StoreCategoryId).ToList();
            var storecategoryIdArr = _calc.GetRandomNumberArray(5, storecategoryIdList);
            //依照IdList取出主分類
            var storecategories = _repo.GetAll<StoreCategory>().Where(sc => storecategoryIdArr.Contains(sc.StoreCategoryId));

            //依照取出主分類找出分類中隨機5家商店
            foreach (var storecategory in storecategories)
            {
                //隨機取出5種商店主分類Id
                var storesIdList = _repo.GetAll<Store>().Where(s => s.StoreCategoryId == storecategory.StoreCategoryId).Select(s => s.StoreId).ToList();
                var storeIdArr = _calc.GetRandomNumberArray(5, storesIdList);

                //依照IdList取出商店
                var stores = _repo.GetAll<Store>().Where(s => storeIdArr.Contains(s.StoreId));

                //將商店資訊存成卡片
                var cards = new List<StoreCard>();              
                foreach (var item in stores)
                {
                    var card = new StoreCard
                    {
                        StoreId = item.StoreId,
                        StoreImg = item.Img,
                        StoreName = item.Name,
                        StoreScore = _storeService.GetStoreAverageScore(item.StoreId),
                        DiscountTag = _storeService.IsDiscountStore(item.StoreId),
                        isFavStore = _productService.ReadHeart(item.StoreId, memberId)
                    };
                    cards.Add(card);
                }
                
                //將卡片加進區塊
                var block = new StoreCardBlock
                {
                    StoreCategoryId = storecategory.StoreCategoryId,
                    StoreCategoryName = storecategory.Name,
                    StoreCards = cards
                };
                result.Add(block);
            }
            return result;
        }
    }
}