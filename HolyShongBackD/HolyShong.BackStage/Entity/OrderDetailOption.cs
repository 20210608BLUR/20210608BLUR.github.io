using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class OrderDetailOption
    {
        public int OrderDetailOptionId { get; set; }
        public int? ProductOptionDetailId { get; set; }
        public int OrderDetailId { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}
