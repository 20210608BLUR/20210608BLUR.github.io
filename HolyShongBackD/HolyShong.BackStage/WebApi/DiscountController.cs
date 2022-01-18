using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.ApiBase;
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
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public OperationResult GetDiscount()
        {
            OperationResult result = new OperationResult();
            try
            {
                var discount = _discountService.GetAllDiscount();

                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = discount;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }
    }
}
