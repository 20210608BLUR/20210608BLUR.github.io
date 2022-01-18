using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Cart
    {
        public Cart()
        {
            Items = new HashSet<Item>();
        }

        public int CartId { get; set; }
        public int MemberId { get; set; }
        public int StroreId { get; set; }
        public int? DiscountMemberId { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
