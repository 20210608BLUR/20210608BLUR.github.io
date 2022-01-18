using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    //public class MemberRegisterViewModel 
    //{
    //    public List<MemberInfo> MemberInfos { get; set; }
    //}
    public class MemberRegisterViewModel
    {
        public int MemberId { get; set; }
        /// <summary>
        /// 會員姓氏
        /// </summary>
        [Required(ErrorMessage = "必須輸入姓氏")]
        [StringLength(50)]
        [Display(Name = "姓氏")]
        public string LastName { get; set; }

        /// <summary>
        /// 會員名字
        /// </summary>
        [Required(ErrorMessage = "必須輸入名字")]
        [StringLength(50)]
        [Display(Name = "名字")]
        public string FirstName { get; set; }

        /// <summary>
        /// 會員手機
        /// </summary>
        [Required(ErrorMessage = "必須輸入手機")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "需輸入10位數字")]
        [Display(Name = "手機")]
        public string Cellphone { get; set; }

        /// <summary>
        /// 會員信箱
        /// </summary>
        [Required(ErrorMessage = "必須輸入信箱")]
        [StringLength(50)]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        /// <summary>
        /// 會員地址
        /// </summary>
        [Required(ErrorMessage = "必須輸入地址")]
        [StringLength(100)]
        [Display(Name = "地址")]
        public string AddressDetails { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        [Required(ErrorMessage = "必須輸入密碼")]
        [StringLength(1280, MinimumLength = 6, ErrorMessage = "需輸入6~20位密碼")]
        //[DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        [Required(ErrorMessage = "必須輸入密碼")]
        [StringLength(1280, MinimumLength = 6, ErrorMessage = "需輸入6~20位密碼")]
        [Compare("Password", ErrorMessage = "兩個密碼不符")]
        //[DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 是否開通帳號(信箱認證)
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否刪除帳號
        /// </summary>
        public bool IsDelete { get; set; }

        public string ActivetionCode { get; set; }

        public DateTime EffectiveTime { get; set; }

        public bool MailIsUsed { get; set; }
    }
}