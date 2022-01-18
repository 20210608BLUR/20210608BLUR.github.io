using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.ApiBase;
using HolyShong.BackStage.ViewModels.Discount;


namespace HolyShong.BackStage.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountAController : ControllerBase
    {
        private readonly IDiscountAService _discountAService;
        public DiscountAController(IDiscountAService discountServiceA)
        {
            _discountAService = discountServiceA;
        }

        [HttpGet]
        public OperationResult GetDiscount()
        {
            OperationResult result = new OperationResult();
            try
            {
                var discountResult = _discountAService.GetAllDiscount();
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = discountResult;
            }

            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }
        [HttpGet]
        public OperationResult GetEffectiveDiscount()
        {
            OperationResult result = new OperationResult();
            try
            {
                var effectiveResult = _discountAService.GetEffectiveDiscount();
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = effectiveResult;

            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;

            }
            return result;
        }

        [HttpGet]
        public OperationResult GetInvalidDiscount()
        {
            var result = new OperationResult();
            try
            {
                var invalidResult = _discountAService.GetInvalidDiscount();
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = invalidResult;

            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;

            }
            return result;
            
        }

        [HttpPost]
        public OperationResult CreateDiscount(CreateDiscountViewModel request)
        {
            OperationResult result = new OperationResult();
            try
            {
                _discountAService.CreateDiscount(request);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = true;

            }

            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = false;
            }
            return result;
        }

        [HttpPost]
        public OperationResult UpdateDiscount(UpdateDiscountViewModel request)
        {
            OperationResult result = new OperationResult();
            try
            {
                _discountAService.UpdateDiscount(request);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = true;

            }

            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = false;
            }
            return result;
        }

        [HttpPost]
        public OperationResult DeleteDiscount(DeleteDiscountViewModel request)
        {
            OperationResult result = new OperationResult();
            try
            {
                _discountAService.DeleteDiscount(request);
                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = true;

            }

            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = false;
            }
            return result;
        }
    }
}
