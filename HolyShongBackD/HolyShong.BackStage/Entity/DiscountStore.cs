using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class DiscountStore
    {
        public int DiscountStoreId { get; set; }
        public int DiscountId { get; set; }
        public int StoreId { get; set; }
        public int UsedNumber { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual Store Store { get; set; }
    }
}
