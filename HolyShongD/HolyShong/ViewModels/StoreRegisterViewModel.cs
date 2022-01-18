using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace HolyShong.ViewModels
{
    public class StoreRegisterViewModel
    {
        /// <summary>
        /// 註冊店名
        /// </summary>
        [Required(ErrorMessage = "店名必填")]
        public string StoreName { get; set; }

        /// <summary>
        /// 註冊店家地址
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string Address { get; set; }

        /// <summary>
        /// 註冊店家電話
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(10, ErrorMessage = "請輸入10位數")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 註冊店家關鍵字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 註冊店家圖片
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 會員Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 已經是店主=>不能再註冊=>提示由後台進入
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>
        /// 店家分類
        /// </summary>
        public int StoreCategoryId { get; set; }

        /// <summary>
        /// 已申請，審核中=>不能再註冊=>提示等3~5天
        /// </summary>
        public bool IsVeryfying { get; set; }
    }
}