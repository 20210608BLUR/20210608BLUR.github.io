namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }

        public int MemberId { get; set; }

        public int? DeliverId { get; set; }

        public int StoreId { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal? Tips { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
