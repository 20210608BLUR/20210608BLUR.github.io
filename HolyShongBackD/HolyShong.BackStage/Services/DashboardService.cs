using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDbRepository _repo;
        public DashboardService(IDbRepository repo)
        {
            _repo = repo;
        }
        public DashboardViewModel GetDashBoardInfo()
        {
            var result = new DashboardViewModel()
            {
                StoreTotal = _repo.GetAll<Member>().Where(m => m.IsStore == true).Count(),
                OrderTotal = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6).Count(),
                MemberTotal = _repo.GetAll<Member>().Where(m => m.IsEnable == true).Count(),
                DiscountTotal = _repo.GetAll<DiscountMember>().Where(dm => dm.IsUsed == true).Count(),
                OrderMonthTotal = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6 && o.RequiredDate.Year == DateTime.UtcNow.AddHours(8).Year && o.RequiredDate.Month == DateTime.UtcNow.AddHours(8).Month).Count(),
                OrderYearTotal = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6 && o.RequiredDate.Year == DateTime.UtcNow.AddHours(8).Year).Count(),
            };
            var monthTotal = (decimal)_repo.GetAll<Order>().Where(o => o.OrderStatus == 6 && o.RequiredDate.Year == DateTime.UtcNow.AddHours(8).Year && o.RequiredDate.Month == DateTime.UtcNow.AddHours(8).Month).Select(o => o.OrginalMoney).Sum();
            var yearTotal = (decimal)_repo.GetAll<Order>().Where(o => o.OrderStatus == 6 && o.RequiredDate.Year == DateTime.UtcNow.AddHours(8).Year).Select(o => o.OrginalMoney).Sum();

            result.EarningMonthTotal = monthTotal.ToString("#,##.##");
            result.EarningYearTotal = yearTotal.ToString("#,##.##");

            return result;
        }

        public List<TopStoreViewModel> AnlsDashBoardInfo()
        {
            var result = new List<TopStoreViewModel>();


            var ordersStoreId = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6).Select(o => o.StoreId);
            var stores = _repo.GetAll<Store>().Where(s => ordersStoreId.Contains(s.StoreId));

            var popularIndex = new Dictionary<Store, int>();
            foreach (var store in stores)
            {
                var orderNums = ordersStoreId.Where(o => o == store.StoreId).Count();
                popularIndex.Add(store, orderNums);
            }
            var popularIndexByOrder = popularIndex.OrderByDescending(x => x.Value).Take(5);
            var i = 1;
            foreach (var item in popularIndexByOrder)
            {
                var orders = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6 && o.StoreId == item.Key.StoreId);
                var temp = new TopStoreViewModel()
                {
                    Index = i,
                    StoreName = item.Key.Name,
                    SellNums = item.Value,
                };
                if (orders.Count() == 0)
                {
                    temp.Sell = 0;
                }
                else
                {
                    temp.Sell = orders.Select(o => o.OrginalMoney).Sum();
                }
                result.Add(temp);
                i++;
            }
            return result;
        }

        public MonthEarningViewModel MonthEarningInfo()
        {
            var result = new MonthEarningViewModel();

            var now = DateTime.UtcNow.AddHours(8);
            var recentlyMonths = new List<DateTime>() { now.AddMonths(-11), now.AddMonths(-10), now.AddMonths(-9), now.AddMonths(-8), now.AddMonths(-7), now.AddMonths(-6), now.AddMonths(-5), now.AddMonths(-4), now.AddMonths(-3), now.AddMonths(-2), now.AddMonths(-1), now };
            var ordersList = _repo.GetAll<Order>().Where(o => o.OrderStatus == 6);
            var temp = new List<decimal>();
            var timeList = new List<string>();
            foreach (var month in recentlyMonths)
            {
                var tempMonth = $"{month.Year}/{month.Month}";
                timeList.Add(tempMonth);
                var monthTotal = ordersList.Where(o => o.RequiredDate.Year == month.Year && o.RequiredDate.Month == month.Month).Select(o => o.OrginalMoney).Sum();
                temp.Add(monthTotal);
                result.Earning = temp;
                result.DateTimeMonths = timeList;
            }
            return result;
        }

        public VipViewModel AnlsVipInfo()
        {
            var result = new VipViewModel()
            {
                IsVip = new List<string>(),
                VipNums = new List<int>(),
            };

            var total = _repo.GetAll<Member>().Where(m => m.IsEnable == true).Count();

            var vips = _repo.GetAll<Rank>().Where(r => r.EndTime != null && r.EndTime >= DateTime.UtcNow.AddHours(8)).Select(r => r.MemberId).Distinct().Count();
            var notvips = total - vips;
            result.VipNums.Add(vips);
            result.VipNums.Add(notvips);

            var vipsPer =((decimal)vips / (decimal)total).ToString("P1");
            var notvipsPer = ((decimal)notvips / (decimal)total).ToString("P1");

            result.IsVip.Add($"送送會員({vipsPer})");
            result.IsVip.Add($"一般會員({notvipsPer})");

            return result;
        }
    }
}
