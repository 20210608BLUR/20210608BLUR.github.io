using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class DiscountMember
    {
        public int DiscountMemberId { get; set; }
        public int DiscountId { get; set; }
        public int MemberId { get; set; }
        public bool IsUsed { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual Member Member { get; set; }
    }
}
