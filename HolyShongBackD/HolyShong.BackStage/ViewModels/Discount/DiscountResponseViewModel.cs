using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Discount
{
    public class DiscountResponseViewModel
    {
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public string DisplayName { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? UseLimit { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Img { get; set; }
        public bool IsAllStore { get; set; }

    }
}
