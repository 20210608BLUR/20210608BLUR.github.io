using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Controllers;
using HolyShong.BackStage.Repositories;
using HolyShong.BackStage.ViewModels.Linebot;
using HolyShong.BackStage.Utility;

namespace HolyShong.BackStage.Services
{
    public class LinebotService : ILinebotService
    {
        private readonly IDbRepository _repo;
        private readonly RandomNum _random;

        public LinebotService(IDbRepository repo)
        {
            _repo = repo;
            _random = new RandomNum();
        }

        public LinebotViewModel SearchRestaurant(string description)
        {
            Store store = new Store();
            var result = new LinebotViewModel();

            //搜尋的是店家
            var stores = _repo.GetAll<Store>().Where(s => s.Name.Contains(description));
            if (stores.Count() != 0)
            {
                List<int> storeIds = stores.Select(s => s.StoreId).ToList();
                result = SearchRandomStore(result, storeIds);
                return result;
            }

            //搜尋的是商品名稱
            var products = _repo.GetAll<Product>().Where(p => p.Name.Contains(description));
            if(products.Count() != 0)
            {
                //找到所有產品的類別
                var productCate = _repo.GetAll<ProductCategory>().Where(pc => products.Select(p=>p.ProductCategoryId).Contains(pc.ProductCategoryId));
                //找到所有產品的店
                List<int> storeIds = _repo.GetAll<Store>().Where(s => productCate.Select(pc=> pc.StoreId).Contains(s.StoreId)).Select(s => s.StoreId).ToList();
                result = SearchRandomStore(result, storeIds);
                return result;
            }


            //搜尋的是店家關鍵字
            var keywordStores = _repo.GetAll<Store>().Where(s=>s.KeyWord.Contains(description));
            if (keywordStores.Count() != 0)
            {
                List<int> storeIds = _repo.GetAll<Store>().Where(s=> keywordStores.Select(ks=>ks.StoreId).Contains(s.StoreId)).Select(s=>s.StoreId).ToList();
                result = SearchRandomStore(result, storeIds);
                return result;
            }

            //搜尋的是類別
            var category = _repo.GetAll<StoreCategory>().FirstOrDefault(sc => sc.Name.Contains(description));
            if(category != null)
            {
                List<int> storeIds = _repo.GetAll<Store>().Where(s => s.StoreCategoryId == category.StoreCategoryId).Select(s => s.StoreId).ToList();
                //類別中亂數推薦一家店
                result = SearchRandomStore(result, storeIds);
                return result;
            }

            //搜尋的是類別關鍵字
            var cateKeyword = _repo.GetAll<StoreCategory>().FirstOrDefault(sc => sc.KeyWord.Contains(description));
            if(cateKeyword != null)
            {
                List<int> storeIds = _repo.GetAll<Store>().Where(s => s.StoreCategoryId == cateKeyword.StoreCategoryId).Select(s => s.StoreId).ToList();
                //類別中亂數推薦一家店
                result = SearchRandomStore(result, storeIds);
                return result;
            }

            result.StoreName = "找不到此關鍵字";
            result.StoreImg = "https://res.cloudinary.com/dvyxx4jau/image/upload/v1635952394/messageImage_1630414816986_kai2_tjtzej.jpg";
            result.StoreUrl = "https://holyshong-frontstage.azurewebsites.net/Home/Index";

            return result;
        }


        /// <summary>
        /// 將結果裝成LINE 卡片JSON
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string RestaurantToJson(LinebotViewModel entity)
        {
            string restaurantJson = $" {{ \"type\": \"bubble\",\"direction\": \"ltr\", \"hero\": {{ \"type\": \"image\", \"url\": \" {entity.StoreImg} \", \"size\": \"full\",\"aspectRatio\": \"20:13\",\"aspectMode\": \"cover\", \"action\": {{ \"type\": \"uri\", \"label\": \"Line\", \"uri\": \"https://linecorp.com/ \"}},\"body\": {{\"type\": \"box\",\"layout\": \"vertical\", \"contents\": [ {{\"type\": \"text\", \"text\": \" {entity.StoreName} \", \"weight\": \"bold\", \"size\": \"xl\", \"contents\": []}}]}}, \"footer\": {{ \"type\": \"box\", \"layout\": \"vertical\", \"flex\": 0, \"spacing\": \"sm\", \"contents\": [{{ \"type\": \"button\", \"action\": {{ \"type\": \"uri\", \"label\": \"WEBSITE\", \"uri\": \"{entity.StoreUrl}\" }}, \"height\": \"sm\", \"style\": \"link\"  }}, {{ \"type\": \"spacer\", \"size\": \"sm\" }} ]}}}}";
            return restaurantJson;
        }

        public LinebotViewModel SearchRandomStore(LinebotViewModel result, List<int> storeIds)
        {
            int[] storeIdArray = _random.GetRandomNumberArray(1, storeIds);
            Store store = _repo.GetAll<Store>().First(s => s.StoreId == storeIdArray[0]);
            result.StoreImg = store.Img;
            result.StoreName = store.Name;
            result.StoreUrl = $"https://holyshong-frontstage.azurewebsites.net/Store/Restaurant/{store.StoreId}";
            return result;
        }
    }
}
