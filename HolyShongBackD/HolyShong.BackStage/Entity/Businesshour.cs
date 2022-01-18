using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Businesshour
    {
        public int BusinesshoursId { get; set; }
        public int StoreId { get; set; }
        public int WeekDay { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual Store Store { get; set; }
    }
}
