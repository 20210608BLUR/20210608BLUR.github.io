using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class HolyCartViewModel
    {
        public int MemberId { get; set; }
        public int RecordId { get; set; }
        /// <summary>
        /// cart
        /// </summary>
        public int CartId { get; set; }
        /// <summary>
        /// store
        /// </summary>
        public string StoreName { get; set; }
        public List<CartItems> CartItems { get; set; }
        public string Address { get; set; }
        public int Count { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsTablewares { get; set; }
        public bool IsPlasticbag { get; set; }
        public string Notes { get; set; }
    }

    public class CartItems
    {
        /// <summary>
        /// item
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// product
        /// </summary>
        public string ProductName { get; set; }
        public List<ProductOptionCards> ProductOptionCards { get; set; }
        /// <summary>
        /// product
        /// </summary>
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// item X product
        /// </summary>
        public decimal TotalPrice { get; set; }

        public string Special { get; set; }
        public int Quantity { get; set; }

    }
    public class ProductOptionCards
    {
        /// <summary>
        /// productOption
        /// </summary>
        public int? ProductOptionId { get; set; }
        /// <summary>
        /// 甜度選擇 productOption
        /// </summary>
        public string ProductOptionName { get; set; }
        /// <summary>
        /// 無糖或少糖 
        /// </summary>
        public string ProductOptionDetail { get; set; }
        /// <summary>
        /// 加價 productOptionDetail
        /// </summary>
        public decimal? addPrice { get; set; }
    }
}
