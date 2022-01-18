using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.ViewModels.ApiBase
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Exception { get; set; }
        public object Result { get; set; }

    }
}
