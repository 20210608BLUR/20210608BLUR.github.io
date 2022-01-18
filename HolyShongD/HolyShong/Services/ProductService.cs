using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static HolyShong.EnumUtility.StatusEnum;

namespace HolyShong.Services
{
    public class ProductService
    {
        private readonly HolyShongRepository _repo;
       
        public ProductService()
        {
            _repo = new HolyShongRepository();
          
        }

        public ProductViewModel GetStore(int storeId,int memberId)
        {

            var result = new ProductViewModel();
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == storeId);
            var storeCategory = _repo.GetAll<StoreCategory>().FirstOrDefault(sc => sc.StoreCategoryId == store.StoreCategoryId);

            result.StoreId = storeId;
            result.StoreName = store.Name;
            result.StoreImg = store.Img;
            result.StoreAddress = store.Address;
            result.StoreCategoryName = storeCategory.Name;
            result.StoreProductCategories = new List<StoreProductCategory>();
            result.isFavStore = ReadHeart(storeId, memberId);
            var supplyTimes = _repo.GetAll<Businesshours>().Where(bh => bh.StoreId == store.StoreId).ToList();
            var supplyTimesByWeekday = String.Join(Environment.NewLine, supplyTimes.OrderBy(x => x.OpenTime).GroupBy(x => x.WeekDay).Select(x => $"{ (WeekDay)x.Key}：{String.Join("、", x.Select(y => $"{y.OpenTime} - {y.CloseTime}"))}").ToList());
            result.SupplyTimes = supplyTimesByWeekday;


            //找店家產品資訊          
            var productCategories = _repo.GetAll<ProductCategory>().Where(pc => pc.StoreId == store.StoreId);
            var products = _repo.GetAll<Product>().Where(p => productCategories.Select(pc => pc.ProductCategoryId).Contains(p.ProductCategoryId));
            var productOption = _repo.GetAll<ProductOption>().Where(po => products.Select(p => p.ProductId).Contains(po.ProductId));
            var productOptionDetail = _repo.GetAll<ProductOptionDetail>().Where(pod => productOption.Select(po => po.ProductOptionId).Contains(pod.ProductOptionId)).ToList();
            //錨點區塊
            foreach (var pc in productCategories.ToList())
            {
                var pcTemp = new StoreProductCategory()
                {
                    StoreProductCategoryName = pc.Name,
                    StoreProductCategorySort = pc.Sort,
                    StoreProducts = new List<StoreProduct>()
                };
                foreach (var p in products.Where(p => p.ProductCategoryId == pc.ProductCategoryId).ToList())
                {
                    var pTemp = new StoreProduct()
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Name,
                        ProductDescription = p.Description,
                        UnitPrice = p.UnitPrice,
                        ProductImg = p.Img,
                        StoreProductOptions = new List<StoreProductOption>()                       

                };
                    pcTemp.StoreProducts.Add(pTemp);
                }
                result.StoreProductCategories.Add(pcTemp);
            }
            var session = HttpContext.Current.Session; //宣告Session 
            session["storeId"] = storeId;

            return result;
        }
        /// <summary>
        /// 讀取喜愛店家
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool ReadHeart(int storeId, int memberId)
        {
            if (memberId==0)
            {
                return false;
            }
            else
            {
                var favStores = _repo.GetAll<Favorite>().Where(fs => fs.StoreId == storeId &&  fs.MemberId == memberId);
                if (favStores.Count() == 0)
                {
                    return false;
                }
                else
                {
                    var isFavStore = _repo.GetAll<Favorite>().Any(fs => fs.StoreId == storeId && fs.MemberId==memberId);
                    return isFavStore;
                }
            }
      

        }
        /// <summary>
        /// 新增喜愛店家 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="storeId"></param>
        public void FavoriteCreate(int memberId, int storeId)
        {
            Favorite fDM = new Favorite();
            fDM.MemberId = memberId;            
            fDM.StoreId = storeId;
            fDM.CreateTime = DateTime.Now;
            _repo.Create(fDM);
            _repo.SaveChange();
        }
        /// <summary>
        /// 刪除喜愛店家 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="memberId"></param>
        public void FavoriteDelete(int storeId,int memberId)
        {
            var deleteFavoriteId = _repo.GetAll<Favorite>().Where(f => f.StoreId == storeId && f.MemberId==memberId);
            if (deleteFavoriteId.FirstOrDefault() == null) return;
            _repo.Delete(deleteFavoriteId.FirstOrDefault());
            _repo.SaveChange();
        }


        public StoreProduct GetStoreProductByProductId(int productId)
        {
            var result = new StoreProduct();

            var p = _repo.GetAll<Product>().FirstOrDefault(x => x.ProductId == productId);

            if (p is null)
            {
                return result;
            }

            var productOptionSource = _repo.GetAll<ProductOption>().Where(po => po.ProductId == productId);
            var productOptionDetail = _repo.GetAll<ProductOptionDetail>().Where(pod => productOptionSource.Select(po => po.ProductOptionId).Contains(pod.ProductOptionId)).ToList();
            var productOption = productOptionSource.ToList();
            var productCategory = _repo.GetAll<ProductCategory>().FirstOrDefault(pc => pc.ProductCategoryId == p.ProductCategoryId);
            var storeName = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == productCategory.StoreId).Name;

            result.StoreName = storeName;
            result.ProductId = p.ProductId;
            result.ProductId = p.ProductId;
            result.ProductName = p.Name;
            result.ProductDescription = p.Description;
            result.UnitPrice = p.UnitPrice;
            result.ProductImg = p.Img;
            result.Quantity = 1;
            result.StoreProductOptions = new List<StoreProductOption>();

            foreach (var po in productOption.ToList())
            {
                var poTemp = new StoreProductOption()
                {
                    ProductOptionName = po.Name,
                    ProductOptionDetails = new List<StoreProductOptionDetail>()
                };
                foreach (var pod in productOptionDetail.Where(x => x.ProductOptionId == po.ProductOptionId))
                {
                    var podTemp = new StoreProductOptionDetail()
                    {
                        StoreProductOptioinDetailName = pod.Name,
                        StoreProductOptionDetailId = pod.ProductOptionDetailId,
                        AddPrice = pod.AddPrice ?? 0
                    };
                    poTemp.ProductOptionDetails.Add(podTemp);
                }
                result.StoreProductOptions.Add(poTemp);
            }

            return result;
        }

        public Store GetStoreByProductId(int productId)
        {
            var product = _repo.GetAll<Product>().First(p => p.ProductId == productId);
            var productCategory = _repo.GetAll<ProductCategory>().First(pc => pc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().First(s => s.StoreId == productCategory.StoreId);
            return store;

        }
    }
}