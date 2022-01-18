using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.Services
{
    public class MarketService
    {
        private readonly HolyShongRepository _repo;
        private readonly ProductService _productService;
        private readonly StoreService _storeService;

        public MarketService()
        {
            _repo = new HolyShongRepository();
            _productService = new ProductService();
            _storeService = new StoreService();
        }
        /// <summary>
        /// 活動店家Read
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns></returns>
        public MarketViewModel GetEventRestaurant(int discountId, int memberId)
        {
            var result = new MarketViewModel();

            //找一個優惠
            var discount = _repo.GetAll<Discount>().FirstOrDefault(d => d.DiscountId == discountId);
            var discountStore = _repo.GetAll<DiscountStore>().Where(ds => ds.DiscountId == discount.DiscountId);
            var store = _repo.GetAll<Store>().Where(s => discountStore.Select(ds => ds.StoreId).Contains(s.StoreId));

            result.EventTitle = discount.Title;
            result.EventContent = discount.Contents;
            //todo 要處理null問題 用value
            result.EventStart = (DateTime)discount.StartTime;
            result.EventEnd = (DateTime)discount.EndTime;

            result.eventRestaurants = new List<EventRestaurant>();

            //卡片區塊
            foreach (var s in store)
            {
                var sTemp = new EventRestaurant()
                {
                    StoreId = s.StoreId,
                    RestaurantImg = s.Img,
                    RestaurantName = s.Name,
                    DiscountTag = _storeService.IsDiscountStore(s.StoreId),
                    StoreScore = _storeService.GetStoreAverageScore(s.StoreId),
                    isFavStore = _productService.ReadHeart(s.StoreId, memberId)
                };
                result.eventRestaurants.Add(sTemp);

            }
            return result;
        }
    }
}