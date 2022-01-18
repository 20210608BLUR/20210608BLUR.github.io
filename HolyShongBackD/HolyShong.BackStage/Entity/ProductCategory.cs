using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int ProductCategoryId { get; set; }
        public int StoreId { get; set; }
        public int Sort { get; set; }
        public string Name { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDelete { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
