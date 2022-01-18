using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Discount
{
    public class CreateDiscountViewModel
    {
        public string DiscountCode { get; set; }
        public string DisplayName { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsAllStore { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
    }
}
