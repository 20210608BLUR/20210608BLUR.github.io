using System.Data.Entity;
using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.Services
{
    public class MemberProfileService
    {
        private readonly HolyShongRepository _repo;
        public MemberProfileService()
        {
            _repo = new HolyShongRepository();
        }

        //取得特定會員Id的Member資料表
        public Member GetMember(int id)
        {
            return _repo.GetAll<Member>().First(x => x.MemberId == id);
        }

        //取得特定會員Id的Rank資料表
        //因為Rank中會有memberId重複，而要取得最新的資料，就要用先排序
        public Rank GetRankByMemberId(int id)
        {
            return _repo.GetAll<Rank>().OrderByDescending(x=>x.EndTime).FirstOrDefault(x => x.MemberId == id);
        }

        //建立ViewModel
        public UserProfileViewModel GetMemberProfileViewModel(int id)
        {            
            var member = GetMember(id);
            bool primary = false;
            Rank rank = GetRankByMemberId(id);
            if (rank != null && rank.IsPrimary)
            {
                primary = true;
            }

            var result = new UserProfileViewModel()
            {
                MemberId = member.MemberId,
                Cellphone = member.Cellphone,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                IsPrimary = primary
            };

            if (rank != null && rank.EndTime.HasValue)
            {
                //會是資料庫的日期減一天(因為資料庫的結束時間是該日的0:00)
                result.PrimaryEndTime = (rank.EndTime.Value).AddDays(-1).ToShortDateString();
            }
            return result;
        }


        /// <summary>
        /// 修改會員資料
        /// </summary>
        /// <param name="memberProfileViewModel"></param>
        /// <returns></returns>
        public bool EditMemberProfile(UserProfileViewModel memberProfileViewModel)
        {
            var member = GetMember(memberProfileViewModel.MemberId);
            member.LastName = memberProfileViewModel.LastName;
            member.FirstName = memberProfileViewModel.FirstName;
            member.Cellphone = memberProfileViewModel.Cellphone;
            member.UpdateTime = DateTime.UtcNow.AddHours(8);
            
            var rank = GetRankByMemberId(memberProfileViewModel.MemberId);

            using (var transaction = _repo.Context.Database.BeginTransaction())
                try
                {
                    _repo.Update<Member>(member);
                    if(rank != null)
                    {
                    _repo.Update<Rank>(rank);
                    }
                    _repo.SaveChange();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
        }
    }
}