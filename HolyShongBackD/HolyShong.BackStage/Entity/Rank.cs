using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Rank
    {
        public int RankId { get; set; }
        public int MemberId { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Member Member { get; set; }
    }
}
