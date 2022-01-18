using HolyShong.BackStage.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services.Interfaces
{
    public interface IDashboardService
    {
        DashboardViewModel GetDashBoardInfo();
        List<TopStoreViewModel> AnlsDashBoardInfo();

        MonthEarningViewModel MonthEarningInfo();

        VipViewModel AnlsVipInfo();
    }
}
