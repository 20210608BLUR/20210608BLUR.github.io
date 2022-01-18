using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Enums;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services
{
    public class MemberService : IMemberService
    {
        private readonly IDbRepository _repo;
        public MemberService(IDbRepository dbRepository)
        {
            _repo = dbRepository;
        }

        public List<DeliverViewModel> GetAllDelivers()
        {
            var result = new List<DeliverViewModel>();
            //找出所有外送員並以DeliverId做排序
            var delivers = _repo.GetAll<Deliver>().OrderBy(d => d.DeliverId).ToList();
            //從外送員找member
            var members = _repo.GetAll<Member>().Where(m => delivers.Select(d => d.MemberId).Contains(m.MemberId)).ToList();
            //找資料
            foreach (var deliver in delivers)
            {
                //先把符合deliver的member挑出來
                var member = members.First(m => m.MemberId == deliver.MemberId);

                //var isDeliver = false;
                //if (member.IsDeliver.HasValue && member.IsDeliver.Value == true)
                //{
                //    isDeliver = true;
                //}

                var tempDeliver = new DeliverViewModel
                {
                    MemberId = deliver.MemberId,
                    LastName = member.LastName,
                    FirstName = member.FirstName,
                    CellPhone = member.Cellphone,
                    Email = member.Email,
                    AuditStatus = member.AuditStatus.HasValue ? (AuditStatus)member.AuditStatus.Value : AuditStatus.NotDeliver,
                    DeliverId = deliver.DeliverId,
                    IsDeliver = member.IsDeliver.HasValue ?  member.IsDeliver.Value : false,
                    IdentityAndLicenseImg=deliver.IdentityAndLicenseImg,
                    InsuranceImg=deliver.InsuranceImg,
                    HeadshotImg=deliver.HeadshotImg,
                    PoliceCriminalRecordImg=deliver.PoliceCriminalRecordImg
                };
                //丟到list
                result.Add(tempDeliver);
            }
            return result;

        }
        public List<RestaurantMemberViewModel> GetAllRestaurantMembers()
        {
            //初始化 List<RestaurantMemberViewModel>
            var result = new List<RestaurantMemberViewModel>();
            //找出所有餐廳
            var restaurants = _repo.GetAll<Store>().ToList();
            //找出member(條件是所有餐廳的memberId)
            var members = _repo.GetAll<Member>().Where(m => restaurants.Select(r => r.MemberId).Contains(m.MemberId)).ToList();
            //對餐廳foreach 
            foreach (var restaurant in restaurants)
            {
                //符合餐廳的member找出來
                var member = members.First(m => m.MemberId == restaurant.MemberId);

                var isStore = false;
                if (member.IsStore.HasValue && member.IsStore.Value == true)
                {
                    isStore = true;
                }

                var tempRestaurant = new RestaurantMemberViewModel
                {
                    RestaurantId = restaurant.StoreId,
                    MemberId = member.MemberId,
                    RestaurantName = restaurant.Name,
                    LastName = member.LastName,
                    FirstName = member.FirstName,
                    IsStore = isStore,
                    StoreAuditStatus = member.StoreAuditStatus.HasValue ? (StoreAuditStatus)member.StoreAuditStatus.Value : StoreAuditStatus.NotStore
                };
                result.Add(tempRestaurant);

            }

            return result;
        }

        public List<MemberViewModel> GetAllMembers()
        {
            var result = new List<MemberViewModel>();
            //找出所有會員
            var members = _repo.GetAll<Member>().ToList();
            //在rank裡面的members且指定的會員們
            var ranks = _repo.GetAll<Rank>()
                .Where(r => members.Select(m => m.MemberId).Contains(r.MemberId)).ToList();
            //找每個會員
            foreach (var item in members)
            {
                var isVip = false;
                //符合rank的member,結束時間從大排到小
                var ranksOfMember = ranks.Where(r => r.MemberId == item.MemberId)
                    .OrderByDescending(x => x.EndTime).ToList();

                //如果找到的一定要有值且rank表裡有memberId且結束時間>現在時間=是vip
                if (ranksOfMember.Count > 0 && ranksOfMember.First().EndTime.HasValue && ranksOfMember.First().EndTime > DateTime.UtcNow.AddHours(8))
                {
                    isVip = true;
                }

                var tempMember = new MemberViewModel
                {
                    MemberId = item.MemberId,
                    LastName = item.LastName,
                    FirstName = item.FirstName,
                    CellPhone = item.Cellphone,
                    Email = item.Email,
                    IsEnable = item.IsEnable,
                    IsVip = isVip 
                };

                result.Add(tempMember);
            }

            return result;
        }

        public string UpdateDeliverAuditStatus(int memberId,bool isAccept)
        {
            var member = _repo.GetAll<Member>().First(m => m.MemberId == memberId);
            member.IsDeliver = isAccept;
            member.AuditStatus = isAccept ? 2 : -1;
            var result = "";           
            using (var transaction = _repo.Context.Database.BeginTransaction()) 
            {               
                try
                {
                    _repo.Update<Member>(member);
                    _repo.Save();
                    transaction.Commit();
                    result = "success";
                }
                catch(Exception ex)
                {
                    result = ex.ToString();
                    transaction.Rollback();
                }
                return result;
            }
        }

        public string UpdateStoreAuditStatus(int memberId, bool isAccept)
        {
            var member = _repo.GetAll<Member>().First(m => m.MemberId == memberId);
            member.IsStore = isAccept;
            member.StoreAuditStatus = isAccept ? 1 : -1;
            var result = "";
            //交易失敗 退回上一步
            using (var transaction = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    _repo.Update<Member>(member);
                    _repo.Save();
                    transaction.Commit();
                    result = "success";
                }
                catch (Exception ex)
                {
                    result = ex.ToString();
                    transaction.Rollback();
                }
                return result;
            }
        }

    }
}
