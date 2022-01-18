using System;
using System.Collections.Generic;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            OrderDetailOptions = new HashSet<OrderDetailOption>();
        }

        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<OrderDetailOption> OrderDetailOptions { get; set; }
    }
}
