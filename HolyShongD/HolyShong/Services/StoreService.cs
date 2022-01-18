using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.ViewModels;
using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.Utility;

namespace HolyShong.Services
{
    public class StoreService
    {
        //初始相依性注入
        private readonly HolyShongRepository _repo;
        private readonly Calculate _calc;
        public StoreService()
        {
            _repo = new HolyShongRepository();
            _calc = new Calculate();
        }

        //關鍵字搜尋方法
        public IQueryable<Store> GetAllStoresByKeyword(string keywordRequest)
        {
            var stores = _repo.GetAll<Store>().Where(x => x.KeyWord.Contains(keywordRequest));
            return stores;
        }

        //類別搜尋方法
        public IEnumerable<Store> GetAllStoresByType(int typeRequest, IQueryable<Store> stores)
        {
            var result = stores.AsEnumerable();
            if (typeRequest == 1)
            {
                result = FavoriteType(stores);
            }
            else if (typeRequest == 2)
            {
                result = PopularType(stores);
            }
            else if (typeRequest == 3)
            {
                result = ScoreType(stores);
            }
            else if (typeRequest == 5)
            {
                result = FavoriteType(stores);
            }
            else if (typeRequest == 6)
            {
                result = PopularType(stores);
            }
            else if (typeRequest == 7)
            {
                result = ScoreType(stores);
            }
            else
            {
                result = _repo.GetAll<Store>().Take(15);
            }
            return result;
        }

        //價格範圍搜尋方法
        public IEnumerable<Store> GetAllStoresByPrice(string priceRequest, IEnumerable<Store> stores)
        {
            var result = stores;
            if (priceRequest == "0-100")
            {
                result = result.Where(s => RangePrice(s.StoreId) >= 0 && RangePrice(s.StoreId) <= 100);
            }
            else if (priceRequest == "100-200")
            {
                result = result.Where(s => RangePrice(s.StoreId) > 100 && RangePrice(s.StoreId) <= 200);
            }
            else if (priceRequest == "200-500")
            {
                result = result.Where(s => RangePrice(s.StoreId) > 200 && RangePrice(s.StoreId) <= 500);
            }
            else
            {
                result = result.Where(s => RangePrice(s.StoreId) > 500);
            }

            return result;
        }

        //類別搜尋方法_精選餐廳
        public IEnumerable<Store> FavoriteType(IQueryable<Store> stores)
        {
            var result = stores;

            //var idList = stores.Select(s=>s.StoreId);
            //var arr = _calc.GetRandomNumberArray(30, idList.ToList());
            //var result = stores.Where(s => arr.Contains(s.StoreId));
            return result;
        }

        //類別搜尋方法_熱門店家
        public IEnumerable<Store> PopularType(IQueryable<Store> stores)
        {
            var ordersStoreId = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6).Select(o => o.StoreId);
            stores = stores.Where(s => ordersStoreId.Contains(s.StoreId));

            var popularIndex = new Dictionary<Store, int>();
            foreach (var store in stores)
            {
                var orderNums = ordersStoreId.Where(o => o == store.StoreId).Count();
                popularIndex.Add(store, orderNums);
            }
            var popularIndexByOrder =  popularIndex.OrderByDescending(x => x.Value);
            var result = popularIndexByOrder.Select(x => x.Key).Take(30);
            return result;
        }

        //類別搜尋方法_評分
        public IEnumerable<Store> ScoreType(IQueryable<Store> stores)
        {
            //過濾店家
            var ordersStoreId = _repo.GetAll<Order>().Where(o => o.Score != null).Select(o => o.StoreId);
            stores = stores.Where(s => ordersStoreId.Contains(s.StoreId));

            var scoreIndex = new Dictionary<Store, decimal>();
            foreach (var store in stores)
            {
                var storeScore = GetStoreAverageScore(store.StoreId);
                scoreIndex.Add(store, storeScore);
            }
            var scoreIndexByOrder = scoreIndex.OrderByDescending(x => x.Value);
            var result = scoreIndexByOrder.Select(x => x.Key).Take(30);
            return result;
        }

        //價格範圍搜尋方法_計算平均價格
        public decimal RangePrice(int storeId)
        {
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == storeId);

            var productcategories = _repo.GetAll<ProductCategory>().Where(pc => pc.StoreId == store.StoreId);

            var products = _repo.GetAll<Product>().Where(p => productcategories.Select(pc => pc.ProductCategoryId).Contains(p.ProductCategoryId));
            if (products.Count() == 0)
            {
                return 0;
            }
            var storeAvgPrice = products.Select(p => p.UnitPrice).Average();

            return storeAvgPrice;
        }

        //計算商店平均分數方法
        public decimal GetStoreAverageScore(int storeId)
        {
            //確定是否有此店家的訂單
            if (_repo.GetAll<Order>().Any(o => o.StoreId == storeId) == false)
            {
                return 0;
            }
            var storeOrders = _repo.GetAll<Order>().Where(o => o.StoreId == storeId && o.Score != null);
            //再確定訂單是否至少有一筆有評分
            if (storeOrders.Count() == 0)
            {
                return 0;
            }
            else
            {
                var scoreAvg = (decimal)storeOrders.Select(so => so.Score).Average();
                return Math.Round(scoreAvg, 1);
            }
        }

        //判斷店家是否為活動店家
        public bool IsDiscountStore(int storeId)
        {
            var result = _repo.GetAll<DiscountStore>().Select(ds => ds.StoreId).Contains(storeId);
            return result;
        }
    }
}