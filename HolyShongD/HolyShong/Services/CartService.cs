using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HolyShong.Services
{
    public class CartService
    {
        private readonly HolyShongRepository _repo;

        public CartService()
        {
            _repo = new HolyShongRepository();
        }

        /// <summary>
        /// 購物車加入商品
        /// </summary>
        /// <param name="productCard"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public Cart AddProductToCart(StoreProduct productCard, int memberId)
        {
            Cart cart = new Cart();
            Item item = new Item();
            //判斷是否有購物車
            var isCart = _repo.GetAll<Cart>().FirstOrDefault(c => c.MemberId == memberId);


            var product = _repo.GetAll<Product>().FirstOrDefault(p => p.ProductId == productCard.ProductId);
            var productCate = _repo.GetAll<ProductCategory>().FirstOrDefault(pc => pc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == productCate.StoreId);
            var productOptions = _repo.GetAll<ProductOption>().Where(po => po.ProductId == product.ProductId);
            var productOptionDetails = _repo.GetAll<ProductOptionDetail>().Where(pod => productOptions.Select(po => po.ProductOptionId).Contains(pod.ProductOptionId));
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    //沒有直接Create購物車
                    if (isCart == null)
                    {
                        cart = new Cart()
                        {
                            MemberId = memberId,
                            StroreId = store.StoreId,
                        };
                        _repo.Create(cart);
                        _repo.SaveChange();

                        item.CartId = cart.CartId;
                    }
                    //有的話讀出並加入新的商品
                    else
                    {
                        item.CartId = isCart.CartId;
                        cart = _repo.GetAll<Cart>().First(c => c.CartId == isCart.CartId);
                    }

                    item.ProductId = productCard.ProductId;
                    item.Quantity = productCard.Quantity;
                    _repo.Create(item);
                    _repo.SaveChange();

                    if (productCard.StoreProductOptions == null)
                    {
                        tran.Commit();
                        return cart;
                    }
                    foreach (var option in productCard.StoreProductOptions)
                    {
                        ItemDetail itemDetail = new ItemDetail()
                        {
                            ItemId = item.ItemId,
                            ProductOptionDetailId = int.Parse(option.SelectOption)
                        };
                        _repo.Create(itemDetail);
                        _repo.SaveChange();
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }

                return cart;
            }

        }

        /// <summary>
        /// 目前購物車內容
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public List<StoreProduct> CurrentCart(Cart cart)
        {
            var items = _repo.GetAll<Item>().Where(i => i.CartId == cart.CartId);
            var itemDetails = _repo.GetAll<ItemDetail>().Where(id => items.Select(i => i.ItemId).Contains(id.ItemId));
            var store = _repo.GetAll<Store>().First(s => s.StoreId == cart.StroreId);
            var products = _repo.GetAll<Product>().Where(p => items.Select(i => i.ProductId).Contains(p.ProductId));
            var productOptions = _repo.GetAll<ProductOption>().Where(po => products.Select(p => p.ProductId).Contains(po.ProductId));
            var productOptionDetails = _repo.GetAll<ProductOptionDetail>().Where(pod => productOptions.Select(po => po.ProductOptionId).Contains(pod.ProductOptionId));


            List<StoreProduct> storeProductList = new List<StoreProduct>();

            foreach (var item in items)
            {
                var product = products.First(p => p.ProductId == item.ProductId);
                var itemDetail = itemDetails.Where(id => id.ItemId == item.ItemId);
                var tempStoreProduct = new StoreProduct()
                {
                    StoreName = store.Name,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductImg = product.Img,
                    UnitPrice = product.UnitPrice,
                    Quantity = item.Quantity,
                    StoreProductOptions = new List<StoreProductOption>()
                };

                foreach (var detail in itemDetail)
                {
                    var productOptionDetail = productOptionDetails.First(pod => pod.ProductOptionDetailId == detail.ProductOptionDetailId);
                    var productOption = productOptions.First(po => po.ProductOptionId == productOptionDetail.ProductOptionId);
                    var tempProductOption = new StoreProductOption()
                    {
                        SelectOption = (productOptionDetail.ProductOptionDetailId).ToString(),
                        SelectOptionPrice = productOptionDetail.AddPrice == null ? 0 : (int)productOptionDetail.AddPrice,
                        ProductOptionName = productOptionDetail.Name,
                        ProductOptionDetails = new List<StoreProductOptionDetail>()
                    };
                    var tempProductOptionDetail = new StoreProductOptionDetail()
                    {
                        StoreProductOptionDetailId = productOptionDetail.ProductOptionDetailId,
                        StoreProductOptioinDetailName = productOptionDetail.Name,
                        AddPrice = productOptionDetail.AddPrice ?? 0
                    };
                    tempProductOption.ProductOptionDetails.Add(tempProductOptionDetail);
                    tempStoreProduct.StoreProductOptions.Add(tempProductOption);
                }
                storeProductList.Add(tempStoreProduct);
            }
            return storeProductList;
        }

        /// <summary>
        /// 尋找會員購物車Cart
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public Cart SearchCart(int memberId)
        {
            Cart cart = new Cart();
            cart = _repo.GetAll<Cart>().FirstOrDefault(c => c.MemberId == memberId);
            if (cart == null)
            {
                return cart;
            }
            return cart;
        }

        /// <summary>
        /// 尋找會員購物車Item
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public List<Item> SearchItems(Cart cart)
        {
            List<Item> items = new List<Item>();
            items = _repo.GetAll<Item>().Where(i => i.CartId == cart.CartId).ToList();
            if (items == null)
            {
                return items;
            }
            return items;
        }

        /// <summary>
        /// 尋找會員購物車ItemDetail
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<ItemDetail> SearchItemDetails(List<Item> items)
        {
            List<ItemDetail> itemDetails = new List<ItemDetail>();
            itemDetails = _repo.GetAll<ItemDetail>().ToList();
            var result = itemDetails.Where(id => items.Select(i => i.ItemId).Contains(id.ItemId)).ToList();
            if (result == null)
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// 刪除購物車
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="items"></param>
        /// <param name="itemDetails"></param>
        public void DeleteCart(Cart cart, List<Item> items, List<ItemDetail> itemDetails)
        {
            foreach (var itemDetail in itemDetails)
            {
                _repo.Delete(itemDetail);
                _repo.SaveChange();
            }
            foreach (var item in items)
            {
                _repo.Delete(item);
                _repo.SaveChange();
            }
            _repo.Delete(cart);
            _repo.SaveChange();
        }

        /// <summary>
        /// 是否為相同店家商品
        /// </summary>
        /// <param name="productCardVM"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool MatchCartStore(List<StoreProduct> productCardVM, int memberId)
        {
            //找出資料庫店家
            var cartStore = _repo.GetAll<Cart>().FirstOrDefault(c => c.MemberId == memberId);

            //Session或新加入商品店家
            var productId = productCardVM[0].ProductId;
            var product = _repo.GetAll<Product>().FirstOrDefault(p => p.ProductId == productId);
            var productCate = _repo.GetAll<ProductCategory>().FirstOrDefault(pc => pc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == productCate.StoreId);

            if(cartStore != null)
            {
                var cartStoreId = cartStore.StroreId;
                var addStoreId = store.StoreId;
                if (cartStoreId == addStoreId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 未登入比較加入商品
        /// </summary>
        /// <param name="storeProducts"></param>
        /// <param name="addProduct"></param>
        /// <returns></returns>
        public bool MatchStoreNoLogin(List<StoreProduct> storeProducts, StoreProduct addProduct)
        {
            if (storeProducts.Count == 0) return true;
            var firstProductId = storeProducts[0].ProductId;
            var product = _repo.GetAll<Product>().FirstOrDefault(p => p.ProductId == firstProductId);
            var productCate = _repo.GetAll<ProductCategory>().FirstOrDefault(pc => pc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == productCate.StoreId).StoreId;

            var addProductId = addProduct.ProductId;
            var findAddProduct = _repo.GetAll<Product>().FirstOrDefault(p => p.ProductId == addProductId);
            var addProductCate = _repo.GetAll<ProductCategory>().FirstOrDefault(pc => pc.ProductCategoryId == findAddProduct.ProductCategoryId);
            var addStore = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == addProductCate.StoreId).StoreId;

            if (store != addStore)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 加入不同商品，更換購物車
        /// </summary>
        /// <param name="storeProducts"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public Cart ChangeCart(List<StoreProduct> storeProducts, int memberId)
        {
            List<Item> findItems = null;
            List<ItemDetail> findItemDetails = null;
            //找到購物車
            var findCart = SearchCart(memberId);
            if(findCart != null)
            {
                findItems = SearchItems(findCart);
                findItemDetails = SearchItemDetails(findItems);
            }
            //刪除購物車
            DeleteCart(findCart, findItems, findItemDetails);
            //重新加入購物車
            Cart cart = new Cart();
            foreach (var storeProduct in storeProducts)
            {
                cart = AddProductToCart(storeProduct, memberId);
            }
            return cart;
        }


        public decimal CalculateMoneyByCart(Cart cart)
        {
            decimal totalPrice = 0;
            decimal addPrice = 0;

            var items = _repo.GetAll<Item>().Where(i => i.CartId == cart.CartId);
            var itemDetails = _repo.GetAll<ItemDetail>().Where(id => items.Select(i => i.ItemId).Contains(id.ItemId));
            var store = _repo.GetAll<Store>().First(s => s.StoreId == cart.StroreId);
            var products = _repo.GetAll<Product>().Where(p => items.Select(i => i.ProductId).Contains(p.ProductId));
            var productOptions = _repo.GetAll<ProductOption>().Where(po => products.Select(p => p.ProductId).Contains(po.ProductId));
            var productOptionDetails = _repo.GetAll<ProductOptionDetail>().Where(pod => productOptions.Select(po => po.ProductOptionId).Contains(pod.ProductOptionId));


            foreach (var item in items)
            {
                var price = products.First(p => p.ProductId == item.ProductId).UnitPrice;
                var itemDetail = itemDetails.Where(id => id.ItemId == item.ItemId);
                if (itemDetail == null) return totalPrice += item.Quantity * price;

                foreach (var detail in itemDetail)
                {
                    var add = productOptionDetails.First(pod => pod.ProductOptionDetailId == detail.ProductOptionDetailId).AddPrice ?? 0;
                    addPrice += add;
                }
                totalPrice += item.Quantity * (price + addPrice);
                addPrice = 0;
            }
            return totalPrice;
        }

        /// <summary>
        /// 結帳頁面畫面呈現
        /// </summary>
        public CartViewModel ToCheckOut(List<StoreProduct> productCard, int memberId)
        {
            CartViewModel cartVM = new CartViewModel();

            var member = _repo.GetAll<Member>().FirstOrDefault(m => m.MemberId == memberId);
            var customerAddress = _repo.GetAll<Address>().FirstOrDefault(a => a.MemberId == member.MemberId && a.IsDefault == true);

            var firstProduct = productCard[0];
            var product = _repo.GetAll<Product>().FirstOrDefault(p => p.ProductId == firstProduct.ProductId);
            var productCate = _repo.GetAll<ProductCategory>().FirstOrDefault(pc => pc.ProductCategoryId == product.ProductCategoryId);
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == productCate.StoreId);

            cartVM.StoreId = store.StoreId;
            cartVM.StoreName = store.Name;
            cartVM.StoreAddress = store.Address;
            cartVM.CustomerAddress = customerAddress.AddressDetail;
            cartVM.Note = "門口碰面";
            cartVM.IsPlasticbag = false;
            cartVM.IsTablewares = false;
            cartVM.CreatedDate = DateTime.UtcNow.AddHours(8);
            cartVM.CartItems = productCard;
            cartVM.DeliverFee = 0;

            //判斷是否為送送會員
            var rank = _repo.GetAll<Rank>().Where(r => r.MemberId == memberId);

            if (rank.ToList().Count == 0)
            {
                cartVM.DeliverFee = 30;
            }
            else
            {
                var rankLevel = rank.OrderByDescending(r=>r.RankId).First();
                if (rankLevel.IsPrimary == false)
                {
                    cartVM.DeliverFee = 30;
                }
            }


            return cartVM;
        }
    }
}