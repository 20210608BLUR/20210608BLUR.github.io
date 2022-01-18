using HolyShong.BackStage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int StoreTotal { get; set; }
        public int OrderTotal { get; set; }
        public int MemberTotal { get; set; }
        public int DiscountTotal { get; set; }
        public string EarningMonthTotal { get; set; }
        public string EarningYearTotal { get; set; }
        public int OrderMonthTotal { get; set; }
        public int OrderYearTotal { get; set; }
    }
}
