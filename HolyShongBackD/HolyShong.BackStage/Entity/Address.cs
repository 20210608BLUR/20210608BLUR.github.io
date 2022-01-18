using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int MemberId { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int ZipCode { get; set; }
        public string AddressDetail { get; set; }

        public virtual Member Member { get; set; }
    }
}
