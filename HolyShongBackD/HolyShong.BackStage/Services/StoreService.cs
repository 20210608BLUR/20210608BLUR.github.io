using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels;
using HolyShong.BackStage.ViewModels.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services
{
    public class StoreService : IStoreService
    {
        private readonly IDbRepository _repo;
        public StoreService(IDbRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// 找分頁商品
        /// </summary>
        /// <param name="pageOnSale"></param>
        /// <returns></returns>
        public PageResult<ProductListViewModel> GetPageProduct(ProductStatusViewModel pageOnSale)
        {
            PageResult<ProductListViewModel> result = new PageResult<ProductListViewModel>();
            var itemCount = 0;
            ////判斷在第幾頁，要從第幾個商品開始取
            if (pageOnSale.CurrentPage != 1)
            {
                itemCount = pageOnSale.PerPage * (pageOnSale.CurrentPage - 1);
            }
            List<ProductListViewModel> productVM = new List<ProductListViewModel>();
            var productList = _repo.GetAll<Product>().Where(p => p.IsEnable == pageOnSale.IsOnSale).ToList();

            var products = _repo.GetAll<Product>().Where(p => p.IsEnable == pageOnSale.IsOnSale).Skip(itemCount).Take(pageOnSale.PerPage);
            var productCates = _repo.GetAll<ProductCategory>().Where(pc => products.Select(p => p.ProductCategoryId).Contains(pc.ProductCategoryId));
            var stores = _repo.GetAll<Store>().Where(s => productCates.Select(pc => pc.StoreId).Contains(s.StoreId));
            var productOptions = _repo.GetAll<ProductOption>().Where(po => products.Select(p => p.ProductId).Contains(po.ProductId));
            var optionDetails = _repo.GetAll<ProductOptionDetail>().Where(od => productOptions.Select(op => op.ProductOptionId).Contains(od.ProductOptionId));



            foreach (var p in products)
            {
                var cate = productCates.First(pc => pc.ProductCategoryId == p.ProductCategoryId);
                var store = stores.First(s => s.StoreId == cate.StoreId);
                var tempProduct = new ProductListViewModel()
                {
                    StoreId = store.StoreId,
                    StoreName = store.Name,
                    ProductCategoryId = p.ProductCategoryId,
                    ProductCategoryName = cate.Name,
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    ProductDescription = p.Description,
                    ProductImg = p.Img ?? string.Empty,
                    UnitPrice = p.UnitPrice,
                    IsEnable = p.IsEnable
                };
                productVM.Add(tempProduct);

            }


            result.Items = productVM;
            result.CurrentPage = pageOnSale.CurrentPage;
            result.TotalRows = productList.Count;

            return result;
        }

        public Array GetProductOptions(int productId)
        {
            var product = _repo.GetAll<Product>().First(p => p.ProductId == productId);
            var productCate = _repo.GetAll<ProductCategory>().First(sc => sc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().First(s => s.StoreId == productCate.StoreId);

            var productCateArray = _repo.GetAll<ProductCategory>().Where(pc => pc.StoreId == store.StoreId).Select(pc => pc.Name).ToList().ToArray();

            return productCateArray;
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <returns></returns>
        public void ProductModify(ProductListViewModel product)
        {
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    //更新產品
                    var findProduct = _repo.GetAll<Product>().First(p => p.ProductId == product.ProductId);
                    findProduct.Name = product.ProductName;
                    findProduct.UnitPrice = product.UnitPrice;
                    findProduct.Description = product.ProductDescription;
                    findProduct.Img = product.ProductImg;

                    //找Category
                    var productCate = _repo.GetAll<ProductCategory>().First(pc => pc.StoreId == product.StoreId && pc.Name.Contains(product.ProductCategoryName));
                    findProduct.ProductCategoryId = productCate.ProductCategoryId;

                    _repo.Update(findProduct);
                    _repo.Save();

                    //有無規格
                    if (product.OptionList.Count == 0)
                    {
                        tran.Commit();
                        return;
                    }

                    //判斷有沒有規格更新
                    var optionChange = product.OptionList[0].OptionDetailList.Where(od=> od.ProductOptionId == 0).ToList();
                        
                    //沒有
                    if (optionChange == null || optionChange.Count == 0)
                    {
                        tran.Commit();
                        return;
                    }

                    var productOption = _repo.GetAll<ProductOption>().Where(po => po.ProductId == product.ProductId);
                    //有，有無一樣的規格
                    foreach (var oc in optionChange)
                    {
                        var option = productOption.FirstOrDefault(po => po.Name.Contains(oc.ProductOptionName));
                        //有，撈出optionID
                        if (option != null)
                        {
                            var detail = new ProductOptionDetail()
                            {
                                ProductOptionId = option.ProductOptionId,
                                Name = oc.ProductOptionDetailName,
                                AddPrice = oc.AddPrice
                            };
                            _repo.Create(detail);
                            _repo.Save();
                        }
                        //沒有一樣，創新的規格類別
                        else
                        {
                            var optionCreate = new ProductOption()
                            {
                                ProductId = product.ProductId,
                                Name = oc.ProductOptionName
                            };
                            _repo.Create(optionCreate);
                            _repo.Save();

                            var detailCreate = new ProductOptionDetail()
                            {
                                ProductOptionId = optionCreate.ProductOptionId,
                                Name = oc.ProductOptionDetailName,
                                AddPrice = oc.AddPrice
                            };
                            _repo.Create(detailCreate);
                            _repo.Save();
                        }
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
            }
        }   


        /// <summary>
        /// 抓全部的餐點
        /// </summary>
        /// <returns></returns>
        public List<ProductListViewModel> GetAllProduct(ProductStatusViewModel pageOnSale)
        {

            if(pageOnSale.Keyword is null)
            {
                pageOnSale.Keyword = string.Empty;
            }

            List<ProductListViewModel> result = new List<ProductListViewModel>();
            var products = _repo.GetAll<Product>().Where(p => p.IsEnable == pageOnSale.IsOnSale);
            var productList = products.ToList();
            var productCates = _repo.GetAll<ProductCategory>().Where(pc => products.Select(p => p.ProductCategoryId).Contains(pc.ProductCategoryId));
            var stores = _repo.GetAll<Store>().Where(s => productCates.Select(pc => pc.StoreId).Contains(s.StoreId)).ToList();
            var productOptions = _repo.GetAll<ProductOption>().Where(po => products.Select(p=>p.ProductId).Contains(po.ProductId));
            var optionDetails = _repo.GetAll<ProductOptionDetail>().Where(od => productOptions.Select(op => op.ProductOptionId).Contains(od.ProductOptionId));

            result = productList.Select(p => new ProductListViewModel() {
                StoreId = stores.First(s => s.StoreId == productCates.First(pc => pc.ProductCategoryId == p.ProductCategoryId).StoreId).StoreId,
                StoreName = stores.First(s => s.StoreId == productCates.First(pc => pc.ProductCategoryId == p.ProductCategoryId).StoreId).Name,
                ProductCategoryId = p.ProductCategoryId,
                ProductCategoryName = productCates.First(pc => pc.ProductCategoryId == p.ProductCategoryId).Name,
                ProductId = p.ProductId,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductImg = p.Img ?? string.Empty,
                UnitPrice = p.UnitPrice,
                IsEnable = p.IsEnable
            }).ToList();


            return result;
        }

        public bool ChangeProductStatus(UpdateProductViewModel updateProduct)
        {
            var product = _repo.GetAll<Product>().First(p => p.ProductId == updateProduct.ID);
            product.IsEnable = updateProduct.SaleStatus;
            _repo.Update(product);
            _repo.Save();
            return updateProduct.SaleStatus;
            
        }

        public ProductListViewModel GetProductDetail(int productId)
        {
            ProductListViewModel result = new ProductListViewModel();
            var product = _repo.GetAll<Product>().FirstOrDefault(p => p.ProductId == productId);
            if(product == null)
            {
                return result;
            }

            var productCate = _repo.GetAll<ProductCategory>().First(pc => pc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().First(s => s.StoreId == productCate.StoreId);

            result.StoreId = store.StoreId;
            result.StoreName = store.Name;
            result.ProductCategoryId = product.ProductCategoryId;
            result.ProductCategoryName = productCate.Name;
            result.ProductId = product.ProductId;
            result.ProductName = product.Name;
            result.ProductImg = product.Img ?? string.Empty;
            result.UnitPrice = product.UnitPrice;
            result.IsEnable = product.IsEnable;
            result.OptionList = new List<ProductOptionViewModel>();

            var productOptions = _repo.GetAll<ProductOption>().Where(po => po.ProductId == productId);
            var productOptionsList = _repo.GetAll<ProductOption>().Where(po => po.ProductId == productId).ToList();
            if (productOptionsList.Count == 0)
            {
                return result;
            }
            var optionDetails = _repo.GetAll<ProductOptionDetail>().Where(od => productOptions.Select(op => op.ProductOptionId).Contains(od.ProductOptionId));

            foreach (var option in productOptionsList)
            {
                var tempOption = new ProductOptionViewModel()
                {
                    ProductOptionId = option.ProductOptionId,
                    ProductOptionName = option.Name,
                    OptionDetailList = new List<ProductOptionDetailViewModel>()
                };

                tempOption.OptionDetailList = optionDetails.Where(od => od.ProductOptionId == option.ProductOptionId).Select(od => new ProductOptionDetailViewModel()
                {
                    ProductOptionId = option.ProductOptionId,
                    ProductOptionName = option.Name,
                    ProductOptionDetailId = od.ProductOptionDetailId,
                    ProductOptionDetailName = od.Name,
                    AddPrice = od.AddPrice ?? 0
                }).ToList();

                result.OptionList.Add(tempOption);
            }

            return result;
        }

        public void DeleteOptionDetail(ProductOptionDetailViewModel optionDetail)
        {
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    var optionCount = _repo.GetAll<ProductOptionDetail>().Where(pod => pod.ProductOptionId == optionDetail.ProductOptionId).ToList().Count;
                    //先刪detail
                    var detail = _repo.GetAll<ProductOptionDetail>().First(pod => pod.ProductOptionDetailId == optionDetail.ProductOptionDetailId);
                    _repo.Delete(detail);
                    _repo.Save();

                     //如果沒有其他detail，刪option
                    if (optionCount == 1)
                    {
                        var option = _repo.GetAll<ProductOption>().First(po => po.ProductOptionId == optionDetail.ProductOptionId);
                        _repo.Delete(option);
                        _repo.Save();
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
            }
        }

        
        public SearchStoreInfo SearchStore(string storeName)
        {
            var result = new SearchStoreInfo();
            var store = _repo.GetAll<Store>().Where(s=> s.Name.Contains(storeName)).ToList();
            if(store == null || store.Count == 0)
            {
                result.ProductCateArray = null;
                result.StoreName = string.Empty;
                result.ResponseMsg = "找不到餐廳";
                return result;
            }
            else if(store.Count > 1)
            {
                result.ProductCateArray = null;
                result.StoreName = string.Empty;
                result.ResponseMsg = "查詢到多間餐廳，請輸入完整店名";
                return result;
            }

          
            result.StoreName = store[0].Name;
            result.ProductCateArray = _repo.GetAll<ProductCategory>().Where(pc => pc.StoreId == store[0].StoreId).Select(pc => pc.Name).ToList().ToArray();
            result.ResponseMsg = "成功找到店家";
            return result;

        }


        public string CreateStore(CreateStoreViewModel createInfo)
        {
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    //尋找storeCategoryID
                    var storeCategory = _repo.GetAll<StoreCategory>().FirstOrDefault(sc => sc.Name.Contains(createInfo.StoreInfo.ChargeName));
                    if (storeCategory == null)
                    {
                        return "找不到此類別";
                    }

                    //找Member改變這個人餐廳狀態
                    var member = _repo.GetAll<Member>().FirstOrDefault(m => m.Cellphone == createInfo.StoreInfo.ChargeName && m.LastName.Contains(createInfo.StoreInfo.ChargeName));
                    if (member == null)
                    {
                        return "找不到此會員，請先進行註冊";
                    }

                    member.IsStore = true;
                    _repo.Update(member);
                    _repo.Save();

                    //創建Store
                    Store store = new Store()
                    {
                        StoreCategoryId = storeCategory.StoreCategoryId,
                        Name = createInfo.StoreInfo.Name,
                        Img = createInfo.StoreInfo.Img,
                        KeyWord = JsonConvert.SerializeObject(createInfo.StoreInfo.Keyword), //null?
                        Address = createInfo.StoreInfo.Address,
                        Cellphone = createInfo.StoreInfo.Phone,
                        CreateTime = DateTime.UtcNow.AddHours(8),
                        MemberId = member.MemberId
                    };

                    _repo.Create(store);
                    _repo.Save();

                    //創建productCategory
                    var cateList = createInfo.CategoryInfo.Select((x, index) => new ProductCategory()
                    {
                        StoreId = store.StoreId,
                        Name = x,
                        Sort = index + 1,
                        IsEnable = true,
                        IsDelete = false
                    }).ToList();

                    _repo.CreateRange(cateList);
                    _repo.Save();

                    return "創建成功";
                    tran.Commit();

                }
                catch
                {
                    return "創建失敗";
                    tran.Rollback();
                }
            }
            

            //var categories = _repo.GetAll<ProductCategory>().Where(pc => pc.StoreId == store.StoreId).ToList();

            ////創建product
            
            //foreach(var product in createInfo.ProductInfo)
            //{
            //    var tempProduct = new Product()
            //    {
            //        ProductCategoryId = categories.First(c => c.Name.Contains(product.ProductCategoryName)).ProductCategoryId,
            //        Name = product.ProductName,
            //        IsPopular = false,
            //        Description = product.ProductDescription,
            //        Img = product.ProductImg == "" ? null : product.ProductImg,
            //        IsEnable = true,
            //        IsDelete = false
            //    };
            //    _repo.Create(tempProduct);
            //    _repo.Save();

            //    //有沒有option
            //    //有，創建option&detail
            //    if (product.OptionList.Length == 0) continue;
            //    //要判斷相同的option

            //    foreach(var options in product.OptionList)
            //    {
            //        var tempOption = new ProductOption()
            //        {
            //            Name = options.ProductOptionName,
            //            ProductId = tempProduct.ProductId
            //        };
            //        _repo.Create(tempOption);
            //        _repo.Save();

            //        var tempDetail = new ProductOptionDetail()
            //        {
            //            ProductOptionId = tempOption.ProductOptionId,
            //            Name = options.ProductOptionDetailName,
            //            AddPrice = decimal.Parse(options.AddPrice)
            //        };
            //        _repo.Create(tempDetail);
            //        _repo.Save();
            //    }

            //}
            //return "成功申請";
        }


        public string CreateProduct(CreateProductViewModel product)
        {
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    //找到類別
                    var inputCatesList = _repo.GetAll<ProductCategory>().Where(pc => pc.Name.Contains(product.ProductCategoryName)).ToList();
                    var inputStoresList = _repo.GetAll<Store>().Where(s => s.Name.Contains(product.StoreName)).ToList();

                    var resultCates = inputCatesList.Where(i => inputStoresList.Select(s => s.StoreId).Contains(i.StoreId)).ToList();
                    var resultCate = inputCatesList.First(i => inputStoresList.Select(s => s.StoreId).Contains(i.StoreId));


                    Product newPro = new Product()
                    {
                        ProductCategoryId = resultCate.ProductCategoryId,
                        Name = product.ProductName,
                        IsPopular = true,
                        Description = product.ProductDescription,
                        UnitPrice = product.UnitPrice,
                        Img = product.ProductImg,
                        IsEnable = true,
                        IsDelete = false
                    };

                    _repo.Create(newPro);
                    _repo.Save();

                    if (product.OptionList == null || product.OptionList.Count == 0)
                    {
                        tran.Commit();
                        return "完成";
                    }

                    //先新增option
                    var optionDistinct = product.OptionList.Select(ol => ol.ProductOptionName).Distinct();

                    foreach (var option in optionDistinct)
                    {
                        ProductOption optionTemp = new ProductOption()
                        {
                            ProductId = newPro.ProductId,
                            Name = option
                        };

                        _repo.Create(optionTemp);
                        _repo.Save();

                        //找此option的OptionList
                        var detailList = product.OptionList.Where(ol => ol.ProductOptionName == option).Select(ol => new ProductOptionDetail()
                        {
                            ProductOptionId = optionTemp.ProductOptionId,
                            Name = ol.ProductOptionDetailName,
                            AddPrice = ol.AddPrice
                        }).ToList();

                        _repo.CreateRange(detailList);
                        _repo.Save();

                    }
                    tran.Commit();
                    return "完成";
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return ex.Message;
                }
            }
            
        }

        //UB
        //public List<ProductListViewModel> GetAllProduct(bool isOnSale)
        //{
        //    List<ProductListViewModel> result = new List<ProductListViewModel>();
        //    var products = _repo.GetAll<Product>().Where(p => p.IsEnable == isOnSale).Take(10);
        //    var productList = products.ToList();
        //    var productCates = _repo.GetAll<ProductCategory>().Where(pc => products.Select(p => p.ProductCategoryId).Contains(pc.ProductCategoryId));
        //    var stores = _repo.GetAll<Store>().Where(s => productCates.Select(pc => pc.StoreId).Contains(s.StoreId)).ToList();
        //    var productOptions = _repo.GetAll<ProductOption>().Where(po => products.Select(p => p.ProductId).Contains(po.ProductId));
        //    var optionDetails = _repo.GetAll<ProductOptionDetail>().Where(od => productOptions.Select(op => op.ProductOptionId).Contains(od.ProductOptionId));


        //    foreach (var p in productList)
        //    {
        //        var productCate = productCates.First(pc => pc.ProductCategoryId == p.ProductCategoryId);
        //        var tempProduct = new ProductListViewModel()
        //        {
        //            StoreId = stores.First(s => s.StoreId == productCate.StoreId).StoreId,
        //            StoreName = stores.First(s => s.StoreId == productCate.StoreId).Name,
        //            ProductCategoryId = p.ProductCategoryId,
        //            ProductCategoryName = productCate.Name,
        //            ProductId = p.ProductId,
        //            ProductName = p.Name,
        //            ProductImg = p.Img ?? string.Empty,
        //            UnitPrice = p.UnitPrice,
        //            IsEnable = p.IsEnable,
        //            OptionList = new List<ProductOptionViewModel>()
        //        };

        //        var options = productOptions.Where(po => po.ProductId == p.ProductId).ToList();
        //        if (options.Count == 0)
        //        {
        //            result.Add(tempProduct);
        //            continue;
        //        }

        //        foreach (var option in options)
        //        {
        //            var tempOption = new ProductOptionViewModel()
        //            {
        //                ProductOptionId = option.ProductOptionId,
        //                ProductOptionName = option.Name,
        //                OptionDetailList = new List<ProductOptionDetailViewModel>()
        //            };

        //            tempOption.OptionDetailList = optionDetails.Where(od => od.ProductOptionId == option.ProductOptionId).Select(od => new ProductOptionDetailViewModel()
        //            {
        //                ProductOptionDetailId = od.ProductOptionDetailId,
        //                ProductOptionDetailName = od.Name,
        //                AddPrice = od.AddPrice ?? 0
        //            }).ToList();

        //            tempProduct.OptionList.Add(tempOption);
        //        }
        //        result.Add(tempProduct);
        //    }
        //    return result;

        //}
    }
}
