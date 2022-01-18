namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Deliver")]
    public partial class Deliver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deliver()
        {
            Order = new HashSet<Order>();
        }

        public int DeliverId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int MemberId { get; set; }

        [Required]
        public string IdentityAndLicenseImg { get; set; }

        [Required]
        public string InsuranceImg { get; set; }

        [Required]
        public string HeadshotImg { get; set; }

        [Required]
        public string PoliceCriminalRecordImg { get; set; }

        public bool isOnline { get; set; }

        public bool isDelivering { get; set; }

        public bool isDelete { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
