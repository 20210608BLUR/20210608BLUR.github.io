namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Favorite")]
    public partial class Favorite
    {
        public int FavoriteId { get; set; }

        public int MemberId { get; set; }

        public int StoreId { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual Member Member { get; set; }

        public virtual Store Store { get; set; }
    }
}
