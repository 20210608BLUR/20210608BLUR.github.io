using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Dashboard
{
    public class TopStoreViewModel
    {
        public int Index { get; set; }
        public string StoreName { get; set; }
        public decimal Sell { get; set; }
        public int SellNums { get; set; }
    }
}
