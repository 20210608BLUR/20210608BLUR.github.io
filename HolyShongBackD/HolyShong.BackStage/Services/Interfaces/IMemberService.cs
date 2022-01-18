using HolyShong.BackStage.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HolyShong.BackStage.Services.Interfaces
{
    public interface IMemberService
    {
        /// <summary>
        /// 取得所有會員
        /// </summary>
        /// <returns>會員們</returns>
        List<MemberViewModel> GetAllMembers();
        /// <summary>
        /// 取得所有外送員
        /// </summary>
        /// <returns></returns>
        List<DeliverViewModel> GetAllDelivers();
        /// <summary>
        /// 取得所有餐廳會員
        /// </summary>
        /// <returns></returns>
        List<RestaurantMemberViewModel> GetAllRestaurantMembers();
        /// <summary>
        /// 更新外送員審核狀態
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        string UpdateDeliverAuditStatus(int memberId,bool isAccept);
        /// <summary>
        /// 更新餐廳審核狀態
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="isAccept"></param>
        /// <returns></returns>
        string UpdateStoreAuditStatus(int memberId, bool isAccept);
    }
}
