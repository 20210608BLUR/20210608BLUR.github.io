namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscountStore")]
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
