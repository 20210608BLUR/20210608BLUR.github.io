using HolyShong.BackStage.Helpers;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.ApiBase;
using HolyShong.BackStage.ViewModels.Member;
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
    public class MemberController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;
        private readonly IMemberService _memberService;       
        public MemberController(IMemberService memberService)
        {
            _jwtHelper = new JwtHelper();
            _memberService = memberService;
        }
        /// <summary>
        /// 找所有一般會員
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OperationResult GetAllMembers()
        {
            OperationResult result = new OperationResult();
            try
            {
                var member = _memberService.GetAllMembers();
                result.IsSuccess = true;
                result.Result = member;
            }
            catch(Exception ex)
            {
                result.Exception = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 找所有外送員
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OperationResult GetAllDelivers()
        {
            OperationResult result = new OperationResult();
            try
            {
                var deliver = _memberService.GetAllDelivers();
                result.IsSuccess = true;
                result.Result = deliver;
            }
            catch(Exception ex)
            {
                result.Exception = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 找所有餐廳會員
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public OperationResult GetAllRestaurantMembers()
        {
            OperationResult result = new OperationResult();
            try
            {
                var restaurantMember = _memberService.GetAllRestaurantMembers();
                result.IsSuccess = true;
                result.Result = restaurantMember;
            }
            catch (Exception ex)
            {
                result.Exception = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 外送員審核狀態
        /// </summary>
        /// <param name="updateDeliverStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public OperationResult IsAcceptAuditStatus(UpdateDeliverStatus updateDeliverStatus)
        {
            OperationResult result = new OperationResult();
            try
            {
                var member = _memberService.UpdateDeliverAuditStatus(updateDeliverStatus.memberId,updateDeliverStatus.IsAccept);
                result.IsSuccess = true;
                result.Result = member;
            }
            catch(Exception ex)
            {
                result.Exception = ex.Message;
            }
            return result;

        }
        /// <summary>
        /// 餐廳審核狀態
        /// </summary>
        /// <param name="updateStoreStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public OperationResult IsAcceptStoreAuditStatus(UpdateStoreStatus updateStoreStatus)
        {
            OperationResult result = new OperationResult();
            try
            {
                var member = _memberService.UpdateStoreAuditStatus(updateStoreStatus.memberId, updateStoreStatus.IsAccept);
                result.IsSuccess = true;
                result.Result = member;
            }
            catch (Exception ex)
            {
                result.Exception = ex.Message;
            }
            return result;

        }
        

    }
}
