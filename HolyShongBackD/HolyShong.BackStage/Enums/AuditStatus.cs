using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Enums
{
    public enum AuditStatus
    {
        /// <summary>
        /// 非外送員
        /// </summary>
        NotDeliver=-99,
        /// <summary>
        /// 拒絕
        /// </summary>
        Reject = -1,
        /// <summary>
        /// 待審核
        /// </summary>
        Pending = 0,
        /// <summary>
        /// 待補件
        /// </summary>
        Lack = 1,
        /// <summary>
        /// 審核完成
        /// </summary>        
        Finish = 2

    }
}
