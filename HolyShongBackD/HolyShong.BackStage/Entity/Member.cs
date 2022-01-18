using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Member
    {
        public Member()
        {
            Addresses = new HashSet<Address>();
            Delivers = new HashSet<Deliver>();
            DiscountMembers = new HashSet<DiscountMember>();
            Favorites = new HashSet<Favorite>();
            Orders = new HashSet<Order>();
            Ranks = new HashSet<Rank>();
        }

        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDelete { get; set; }
        public bool? IsDeliver { get; set; }
        public bool? IsStore { get; set; }
        public Guid? ActivetionCode { get; set; }
        public DateTime? EffectiveTime { get; set; }
        public int? AuditStatus { get; set; }
        public int? StoreAuditStatus { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Deliver> Delivers { get; set; }
        public virtual ICollection<DiscountMember> DiscountMembers { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rank> Ranks { get; set; }
    }
}
