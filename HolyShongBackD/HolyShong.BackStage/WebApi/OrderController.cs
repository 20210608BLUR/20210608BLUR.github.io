using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.ApiBase;
using HolyShong.BackStage.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;

namespace HolyShong.BackStage.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        
        [HttpGet]
        public OperationResult GetPreparingOrders()
        {
            OperationResult result = new OperationResult();
            try
            {
                List<int> preparingStatus = new List<int>() { 1, 2 };
                var preparingOrders = _orderService.GetOrdersByStatus(preparingStatus);

                result.IsSuccess = true;
                result.Result = preparingOrders;
                result.Exception = string.Empty;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Result = null;
                result.Exception = ex.ToString();

            }
            return result;
        }

        
        [HttpGet]
        public OperationResult GetHistoryOrders()
        {
            OperationResult result = new OperationResult();
            try
            {
                List<int> historyStatus = new List<int>() { 3, 4, 5, 6 };
                var historyOrders = _orderService.GetOrdersByStatus(historyStatus);

                result.IsSuccess = true;
                result.Result = historyOrders;
                result.Exception = string.Empty;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Result = null;
                result.Exception = ex.ToString();

            }
            return result;
        }

        
        [HttpPost]
        public OperationResult UpdateOrder(OrderStatusResponseViewModel request)
        {
            OperationResult result = new OperationResult();
            var updateResult = _orderService.UpdateOrder(request);
            if (updateResult == "success")
            {
                result.IsSuccess = true;
                result.Result = updateResult;
                result.Exception = string.Empty;
            }
            else
            {
                result.IsSuccess = false;
                result.Result = null;
                result.Exception = updateResult;
            }

            return result;
        }
    }

}
