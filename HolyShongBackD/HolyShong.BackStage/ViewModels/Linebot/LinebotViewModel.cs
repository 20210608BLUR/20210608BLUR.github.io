using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.Linebot
{
    public class LinebotViewModel
    {
        public string StoreName { get; set; }
        public string StoreUrl { get; set; }
        public string StoreImg { get; set; }
    }

    public class LineBotRequestViewModel
    {
        public string description { get; set; }
    }
}
