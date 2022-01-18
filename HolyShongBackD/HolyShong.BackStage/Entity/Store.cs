using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Store
    {
        public Store()
        {
            Businesshours = new HashSet<Businesshour>();
            DiscountStores = new HashSet<DiscountStore>();
            Favorites = new HashSet<Favorite>();
            ProductCategories = new HashSet<ProductCategory>();
        }

        public int StoreId { get; set; }
        public int? StoreCategoryId { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string KeyWord { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsDelete { get; set; }
        public int? MemberId { get; set; }

        public virtual ICollection<Businesshour> Businesshours { get; set; }
        public virtual ICollection<DiscountStore> DiscountStores { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
