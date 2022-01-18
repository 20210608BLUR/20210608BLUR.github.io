using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.ApiBase;
using HolyShong.BackStage.ViewModels.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public OperationResult GetProductList([FromQuery]ProductStatusViewModel onSale)
        {
            OperationResult result = new OperationResult();
            try
            {
                var products = _storeService.GetPageProduct(onSale);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = products;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }


        [HttpPost]
        public OperationResult UpdateProductSalesStatus(UpdateProductViewModel data)
        {
            OperationResult result = new OperationResult();
            try
            {
                var status = _storeService.ChangeProductStatus(data);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = status;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }

        [HttpGet]
        public OperationResult GetProductDetails([FromQuery]int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var status = _storeService.GetProductDetail(id);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = status;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }

        [HttpGet]
        public OperationResult GetProductOptions([FromQuery] int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var optionsArray = _storeService.GetProductOptions(id);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = optionsArray;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }

        [HttpPost]
        public OperationResult ProductModify(ProductListViewModel product)
        {
            OperationResult result = new OperationResult();
            try
            {
                _storeService.ProductModify(product);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = null;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }

        [HttpPost]
        public OperationResult DeleteOption(ProductOptionDetailViewModel optionDetail)
        {
            OperationResult result = new OperationResult();
            try
            {
                _storeService.DeleteOptionDetail(optionDetail);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = null;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }

        [HttpPost]
        public OperationResult CreateStore(CreateStoreViewModel createInfo)
        {
            OperationResult result = new OperationResult();
            try
            {
                //_storeService.CreateStore(createInfo);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = null;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }


        [HttpPost]
        public OperationResult CreateProduct(CreateProductViewModel product)
        {
            OperationResult result = new OperationResult();
            try
            {
                _storeService.CreateProduct(product);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = null;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }


        [HttpPost]
        public OperationResult SearchStore(SearchStore storeName)
        {
            OperationResult result = new OperationResult();
            try
            {
                var response = _storeService.SearchStore(storeName.StoreName);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = response;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }
    }
}
