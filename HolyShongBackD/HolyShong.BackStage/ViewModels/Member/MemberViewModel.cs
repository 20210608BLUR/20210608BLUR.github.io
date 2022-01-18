using HolyShong.BackStage.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Member
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        /// <summary>
        /// 是否為Vip
        /// </summary>
        public bool IsVip { get; set; }
        /// <summary>
        /// 是否按下認證信
        /// </summary>
        public bool IsEnable { get; set; }
        public string AddressDetail { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class DeliverViewModel
    {
        public int MemberId { get; set; }
        public int DeliverId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        /// <summary>
        /// 外送員審核狀態
        /// </summary>
        public AuditStatus AuditStatus { get; set; }
 
        public bool IsDeliver { get; set; }
        /// <summary>
        /// 身分證&駕照
        /// </summary>
        public string IdentityAndLicenseImg { get; set; }
        /// <summary>
        /// 強制險
        /// </summary>
        public string InsuranceImg { get; set; }
        /// <summary>
        /// 頭像
        /// </summary>
        public string HeadshotImg { get; set; }
        /// <summary>
        /// 良民證
        /// </summary>
        public string PoliceCriminalRecordImg { get; set; }
        
    }
    public class RestaurantMemberViewModel
    {
        public int RestaurantId{ get; set; }
        public int MemberId { get; set; }
        public string RestaurantName { get; set; }       
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsStore { get; set; }
        /// <summary>
        /// 餐廳審核狀態
        /// </summary>
        public StoreAuditStatus StoreAuditStatus { get; set; }
    }
    /// <summary>
    /// 外送員審核狀態
    /// </summary>
    public class UpdateDeliverStatus
    {
        public int memberId { get; set; }
        public bool IsAccept { get; set; }
    }
    /// <summary>
    /// 餐廳審核狀態
    /// </summary>
    public class UpdateStoreStatus
    {
        public int memberId { get; set; }
        public bool IsAccept { get; set; }
    }
}
