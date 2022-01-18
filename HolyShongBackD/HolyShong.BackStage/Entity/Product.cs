using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Product
    {
        public Product()
        {
            Items = new HashSet<Item>();
            ProductOptions = new HashSet<ProductOption>();
        }

        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsPopular { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string Img { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDelete { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ProductOption> ProductOptions { get; set; }
    }
}
