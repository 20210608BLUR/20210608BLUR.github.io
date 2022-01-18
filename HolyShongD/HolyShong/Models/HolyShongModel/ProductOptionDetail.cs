namespace HolyShong.Models.HolyShongModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductOptionDetail")]
    public partial class ProductOptionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductOptionDetail()
        {
            ItemDetail = new HashSet<ItemDetail>();
        }

        public int ProductOptionDetailId { get; set; }

        public int ProductOptionId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public decimal? AddPrice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemDetail> ItemDetail { get; set; }

        public virtual ProductOption ProductOption { get; set; }
    }
}
