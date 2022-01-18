using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HolyShong.ViewModels
{
    public class MemberLoginViewModel
    {
        /// <summary>
        /// 會員信箱
        /// </summary>
        [Required(ErrorMessage = "必須輸入信箱")]
        [StringLength(50)]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        [Required(ErrorMessage = "必須輸入密碼")]
        [StringLength(1280, MinimumLength = 6, ErrorMessage = "需輸入6~20位密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "記得我")]
        public bool Remember { get; set; }

        public string Url { get; set; }
    }
}