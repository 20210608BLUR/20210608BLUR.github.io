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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet]
        public OperationResult GetInfo()
        {
            OperationResult result = new OperationResult();
            try
            {
                var dashboard = _dashboardService.GetDashBoardInfo();

                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = dashboard;
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
        public OperationResult AnlsInfo()
        {
            OperationResult result = new OperationResult();
            try
            {
                var dashboard = _dashboardService.AnlsDashBoardInfo();

                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = dashboard;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }

        public OperationResult EarningInfo()
        {
            OperationResult result = new OperationResult();
            try
            {
                var dashboard = _dashboardService.MonthEarningInfo();

                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = dashboard;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Exception = ex.Message;
                result.Result = null;
            }
            return result;
        }
        public OperationResult VipInfo()
        {
            OperationResult result = new OperationResult();
            try
            {
                var dashboard = _dashboardService.AnlsVipInfo();

                result.IsSuccess = true;
                result.Exception = string.Empty;
                result.Result = dashboard;
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
