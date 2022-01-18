using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class DeliverRegisterViewModel
    {
        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 身分證及駕照
        /// </summary>
        public string IdentityAndLicenseImg { get; set; }

        /// <summary>
        /// 機車強制險
        /// </summary>
        public string InsuranceImg { get; set; }

        /// <summary>
        /// 大頭照
        /// </summary>
        public string HeadshotImg { get; set; }

        /// <summary>
        /// 良民證
        /// </summary>
        public string PoliceCriminalRecordImg { get; set; }

        /// <summary>
        /// 是否上線中，註冊不使用
        /// </summary>
        public bool isOnline { get; set; }

        /// <summary>
        /// 是否外送中，註冊不使用
        /// </summary>
        public bool isDelivering { get; set; }

        /// <summary>
        /// 是否刪除此外送員資格，註冊不使用
        /// </summary>
        public bool isDelete { get; set; }

        /// <summary>
        /// 已經是外送員=>導向訂單頁面
        /// </summary>
        public bool IsDeliver { get; set; }

        /// <summary>
        /// 已註冊，審核中=>顯示需要時間審核
        /// </summary>
        public bool IsVerifying { get; set; }
    }
}