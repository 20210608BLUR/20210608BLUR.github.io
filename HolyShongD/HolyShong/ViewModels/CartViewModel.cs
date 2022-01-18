using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.ViewModels
{
    public class CartViewModel
    {

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string Note { get; set; }
        public string CustomerAddress { get; set; }
        public bool IsTablewares { get; set; }
        public bool IsPlasticbag { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal DeliverFee { get; set; }
        public List<StoreProduct> CartItems { get; set; }
    }

    public class CartItem : StoreProduct
    {

    }
}