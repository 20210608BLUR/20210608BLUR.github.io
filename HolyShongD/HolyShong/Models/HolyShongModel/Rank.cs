namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rank")]
    public partial class Rank
    {
        public int RankId { get; set; }

        public int MemberId { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime? EndTime { get; set; }

        public virtual Member Member { get; set; }
    }
}
