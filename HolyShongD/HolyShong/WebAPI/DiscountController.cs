using HolyShong.Services;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HolyShong.WebAPI
{
    public class DiscountController : ApiController
    {
        private readonly DiscountService _discountService;
        private readonly ProductService _productService;

        public DiscountController()
        {
            _discountService = new DiscountService();
            _productService = new ProductService();
        }

        [HttpPost]
        public OperationResult inputDiscountCode(DiscountCodeViewModel discountCode)
        {
            var memberId = int.Parse(User.Identity.Name);
            OperationResult result = new OperationResult();

            try
            {
                var discount = _discountService.AcquireDiscount(discountCode.DiscountCode, memberId);

                result.IsSuccessful = true;
                result.Exception = String.Empty;
                result.Result = discount;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Exception = ex.ToString();
                result.Result = null;
            }
            return result;
        }


        /// <summary>
        /// 取得會員優惠卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public OperationResult GetDiscount()
        {
            var memberId = int.Parse(User.Identity.Name);
            OperationResult result = new OperationResult();
            List<DiscountViewModel> discountVM = new List<DiscountViewModel>();

            try
            {
                discountVM = _discountService.GetDiscountByMemberId(memberId, 0);
                result.IsSuccessful = true;
                result.Exception = String.Empty;
                result.Result = discountVM;
            }
            catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.Exception = ex.ToString();
                result.Result = null;
            }

            return result;
        }


        /// <summary>
        /// 確認此消費店家可用優惠卷
        /// </summary>
        /// <param name="storeProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public OperationResult MatchDiscountStore(List<StoreProduct> storeProduct)
        {
            var memberId = int.Parse(User.Identity.Name);
            var storeId = 0;
            if (storeProduct == null)
            {
                storeId = 0;
            }
            else
            {
                var productId = storeProduct[0].ProductId;
                storeId = _productService.GetStoreByProductId(productId).StoreId;
            }

            OperationResult result = new OperationResult();
            List<DiscountViewModel> discountVM = new List<DiscountViewModel>();
            try
            {
                discountVM = _discountService.GetDiscountByMemberId(memberId, storeId);
                result.IsSuccessful = true;
                result.Exception = String.Empty;
                result.Result = discountVM;
            }
            catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.Exception = ex.ToString();
                result.Result = null;
            }
            return result;
        }

    }
}
