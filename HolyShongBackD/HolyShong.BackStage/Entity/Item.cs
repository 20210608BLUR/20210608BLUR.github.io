using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Item
    {
        public Item()
        {
            ItemDetails = new HashSet<ItemDetail>();
        }

        public int ItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ItemDetail> ItemDetails { get; set; }
    }
}
