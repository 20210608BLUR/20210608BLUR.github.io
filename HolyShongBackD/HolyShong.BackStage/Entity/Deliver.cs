using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Deliver
    {
        public Deliver()
        {
            Orders = new HashSet<Order>();
        }

        public int DeliverId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int MemberId { get; set; }
        public string IdentityAndLicenseImg { get; set; }
        public string InsuranceImg { get; set; }
        public string HeadshotImg { get; set; }
        public string PoliceCriminalRecordImg { get; set; }
        public bool IsOnline { get; set; }
        public bool IsDelivering { get; set; }
        public bool IsDelete { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
