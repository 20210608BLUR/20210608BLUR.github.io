using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class ProductOption
    {
        public ProductOption()
        {
            ProductOptionDetails = new HashSet<ProductOptionDetail>();
        }

        public int ProductOptionId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<ProductOptionDetail> ProductOptionDetails { get; set; }
    }
}
