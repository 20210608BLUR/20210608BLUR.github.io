using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.ApiBase;
using HolyShong.BackStage.ViewModels.Linebot;
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
    public class LinebotController : ControllerBase
    {
        private readonly ILinebotService _linebotService;
        public LinebotController(ILinebotService linebotService)
        {
            _linebotService = linebotService;
        }

        [HttpPost]
        public OperationResult GetStoreByDescription(LineBotRequestViewModel des)
        {
            var request = des.description.Trim();
            OperationResult result = new OperationResult();
            try
            {
                var discount = _linebotService.SearchRestaurant(request);

                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = discount;
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
