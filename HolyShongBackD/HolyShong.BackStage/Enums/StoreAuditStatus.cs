using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Enums
{
    public enum StoreAuditStatus
    {
        /// <summary>
        /// 非餐廳會員
        /// </summary>
        NotStore = -99,
        /// <summary>
        /// 拒絕
        /// </summary>
        Reject = -1,
        /// <summary>
        /// 待審核
        /// </summary>
        Pending = 0,       
        /// <summary>
        /// 審核完成
        /// </summary>        
        Finish = 1
    }
}
