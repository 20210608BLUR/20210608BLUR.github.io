using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Dashboard
{
    public class MonthEarningViewModel
    {
        public List<string> DateTimeMonths { get; set; }
        public  List<decimal> Earning { get; set; }
    }
}
