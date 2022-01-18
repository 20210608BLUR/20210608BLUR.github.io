using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int? DeliverId { get; set; }
        public int StoreId { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal? Tips { get; set; }
        public string Notes { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsTablewares { get; set; }
        public bool IsPlasticbag { get; set; }
        public int PaymentStatus { get; set; }
        public int DeliverStatus { get; set; }
        public int OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public int? MemberDiscountId { get; set; }
        public DateTime? OrderStatusUpdateTime { get; set; }
        public int? Score { get; set; }
        public decimal? DiscountMoney { get; set; }
        public decimal OrginalMoney { get; set; }

        public virtual Deliver Deliver { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
