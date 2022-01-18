namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetailOption")]
    public partial class OrderDetailOption
    {
        public int OrderDetailOptionId { get; set; }

        public int? ProductOptionDetailId { get; set; }

        public int OrderDetailId { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}
