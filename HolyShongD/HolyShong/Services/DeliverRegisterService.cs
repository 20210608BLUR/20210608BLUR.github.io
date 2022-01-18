using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.Repositories;
using HolyShong.Models.HolyShongModel;
using HolyShong.ViewModels;
using System.Data.Entity;

namespace HolyShong.Services
{
    public class DeliverRegisterService
    {
        private readonly HolyShongRepository _repo;

        public DeliverRegisterService()
        {
            _repo = new HolyShongRepository();
        }

        /// <summary>
        /// 確認外送員狀態
        /// 是外送員=>導向訂單頁面
        /// 已申請外送員，尚在審核中=>提示已申請過
        /// 第一次申請=>給申請表
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public DeliverRegisterViewModel CheckStatus(int memberId)
        {
            var deliverRegisterViewModel = new DeliverRegisterViewModel() { MemberId = memberId };
            var member = _repo.GetAll<Member>().First(x => x.MemberId == memberId);
            var deliver = _repo.GetAll<Deliver>().FirstOrDefault(x => x.MemberId == memberId);
            //已經是外送員
            if (member.IsDeliver == true)
            {
                deliverRegisterViewModel.IsDeliver = true;
            }
            //不是外送員，但已經申請
            else if (deliver != null)
            {
                deliverRegisterViewModel.IsVerifying = true;
            }

            return deliverRegisterViewModel;
        }


        public bool CreateDeliver(DeliverRegisterViewModel request)
        {

            var member = _repo.GetAll<Member>().First(x => x.MemberId == request.MemberId);
            member.AuditStatus = 0;


            var deliver = new Deliver()
            {
                MemberId = request.MemberId,
                CreateTime = DateTime.UtcNow.AddHours(8),
                IdentityAndLicenseImg = request.IdentityAndLicenseImg,
                InsuranceImg = request.InsuranceImg,
                HeadshotImg = request.HeadshotImg,
                PoliceCriminalRecordImg = request.PoliceCriminalRecordImg,
                isOnline = false,
                isDelivering = false,
                isDelete = false,
            };
            DbContext context = new HolyShongContext();
            using (var transation = context.Database.BeginTransaction())
            {
                try {
                    _repo.Create<Deliver>(deliver);
                    _repo.Update<Member>(member);
                    _repo.SaveChange();
                    transation.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    string errorString = ex.ToString();
                    return false;
                }
            }

        }
    }
}