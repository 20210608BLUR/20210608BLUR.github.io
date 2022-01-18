using HolyShong.BackStage.ViewModels;
using HolyShong.BackStage.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services.Interfaces
{
    public interface IStoreService
    {
        public PageResult<ProductListViewModel> GetPageProduct(ProductStatusViewModel pageOnSale);
        public List<ProductListViewModel> GetAllProduct(ProductStatusViewModel pageOnSale);
        public bool ChangeProductStatus(UpdateProductViewModel updateProduct);
        public ProductListViewModel GetProductDetail(int productId);
        public void DeleteOptionDetail(ProductOptionDetailViewModel optionDetail);
        public Array GetProductOptions(int productId);
        public void ProductModify(ProductListViewModel product);
        public SearchStoreInfo SearchStore(string storeName);

        public string CreateProduct(CreateProductViewModel product);
        //public void CreateStore(CreateStoreViewModel createInfo);
    }
}
