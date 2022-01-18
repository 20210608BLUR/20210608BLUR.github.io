using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class AddressViewModel
    {
        /// 新增地址

        /// <summary>
        /// 地址Id
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// 會員ID
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 是否為預設顯示地址
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 創立地址時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 地址變更時間
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 郵遞區號-3碼
        /// </summary>
        public int Zipcode { get; set; }

        /// <summary>
        /// 詳細地址
        /// </summary>
        // public String AddressDetails { get; set; }

        public List<AddressDetail> AddressDetails { get; set; }
    }

    public class AddressDetail
    {
        public bool IsDefault { get; set; }

        public string AllAddress { get; set; }

        public int AddressId { get; set; }

    }
}