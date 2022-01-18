namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Discount")]
    public partial class Discount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discount()
        {
            DiscountMember = new HashSet<DiscountMember>();
            DiscountStore = new HashSet<DiscountStore>();
        }

        public int DiscountId { get; set; }

        [Required]
        [StringLength(20)]
        public string DiscountCode { get; set; }

        [Required]
        [StringLength(20)]
        public string DisplayName { get; set; }

        public int Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? UseLimit { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public string Img { get; set; }

        public bool isAllStore { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscountMember> DiscountMember { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscountStore> DiscountStore { get; set; }
    }
}
