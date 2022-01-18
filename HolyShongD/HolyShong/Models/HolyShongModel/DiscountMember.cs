namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscountMember")]
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
