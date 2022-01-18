using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class ItemDetail
    {
        public int ItemDetailId { get; set; }
        public int ItemId { get; set; }
        public int? ProductOptionDetailId { get; set; }

        public virtual Item Item { get; set; }
        public virtual ProductOptionDetail ProductOptionDetail { get; set; }
    }
}
