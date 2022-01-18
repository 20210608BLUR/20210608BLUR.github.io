using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class ProductOptionDetail
    {
        public ProductOptionDetail()
        {
            ItemDetails = new HashSet<ItemDetail>();
        }

        public int ProductOptionDetailId { get; set; }
        public int ProductOptionId { get; set; }
        public string Name { get; set; }
        public decimal? AddPrice { get; set; }

        public virtual ProductOption ProductOption { get; set; }
        public virtual ICollection<ItemDetail> ItemDetails { get; set; }
    }
}
