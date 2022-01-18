using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.Services
{
    public class FavoriteService
    {
        private readonly HolyShongRepository _repo;
        private readonly StoreService _storeService;

        public FavoriteService()
        {
            _repo = new HolyShongRepository();
            _storeService = new StoreService();
        }
        /// <summary>
        /// 最愛店家頁面
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public FavoriteViewModel GetFavorite(int memberId)
        {
            var result = new FavoriteViewModel();
            var member = _repo.GetAll<Member>().FirstOrDefault(m => m.MemberId == memberId);
            var favorite = _repo.GetAll<Favorite>().Where(f => f.MemberId == memberId).OrderByDescending(f => f.CreateTime);
            var store = _repo.GetAll<Store>().Where(s => favorite.Select(f => f.StoreId).Contains(s.StoreId)).ToList()
                .Select(s => new
                {
                    StoreId = s.StoreId,
                    Img = s.Img,
                    Name = s.Name,
                    Index = favorite.ToList().FindIndex(f => f.StoreId == s.StoreId)
                }).OrderBy(s => s.Index);

            result.favoriteStores = new List<FavoriteStore>();

            foreach (var s in store)
            {
                var sTemp = new FavoriteStore()
                {
                    StoreId = s.StoreId,
                    StoreImg = s.Img,
                    StoreName = s.Name,
                    DiscountTag = _storeService.IsDiscountStore(s.StoreId),
                    StoreScore = _storeService.GetStoreAverageScore(s.StoreId),
                };
                result.favoriteStores.Add(sTemp);
            }

            return result;
        }
    }
}