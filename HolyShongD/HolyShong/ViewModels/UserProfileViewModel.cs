using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    /// <summary>
    /// 會員頁面
    /// 用於呈現會員個人資料
    /// 使用資料表：
    /// Member
    /// Rank
    /// </summary>
    public class UserProfileViewModel
    {

        /// <summary>
        /// 會員編號
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 會員姓氏
        /// </summary>
        [Required(ErrorMessage ="姓氏必塡")]
        [StringLength(50,MinimumLength =1,ErrorMessage = "不得為空白,至少1字元")]
        public string LastName { get; set; }

        /// <summary>
        /// 會員名字
        /// </summary>
        [Required]
        [StringLength(50,MinimumLength =1, ErrorMessage = "不得為空白,至少1字元")]
        public string FirstName { get; set; }

        /// <summary>
        /// 會員手機
        /// </summary>
        [Required]
        [StringLength(10,MinimumLength =10,ErrorMessage ="請輸入10位數")]
        public string Cellphone { get; set; }

        /// <summary>
        /// 送送會員
        /// 對應Rank表
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// 送送會員到期時間
        /// 對應Rank表
        /// </summary>
        public string PrimaryEndTime { get; set; }

        /// <summary>
        /// 會員信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 會員資料更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public bool IsUpdate { get; set; }
    }

}

