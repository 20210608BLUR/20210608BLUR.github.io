using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
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
