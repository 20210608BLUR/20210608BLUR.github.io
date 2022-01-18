using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels
{
    public class PageResult<T> where T : class
    {
        public List<T> Items { get; set; }

        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

    }
}
