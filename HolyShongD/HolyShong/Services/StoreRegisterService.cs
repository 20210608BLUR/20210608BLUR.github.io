using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System.Data.Entity;

namespace HolyShong.Services
{
    public class StoreRegisterService
    {
        private readonly HolyShongRepository _repo;

        public StoreRegisterService()
        {
            _repo = new HolyShongRepository();
        }

        public StoreRegisterViewModel MemberStatus(int memberId)
        {
            var member = _repo.GetAll<Member>().First(x => x.MemberId == memberId);
            var store = _repo.GetAll<Store>().FirstOrDefault(x => x.MemberId == memberId);
            var storemember = new StoreRegisterViewModel();
            storemember.MemberId = memberId; ;
            //已經是店主
            if (member.IsStore == true)
            {
                storemember.IsOwner = true;
            }
            //已註冊，審核中
            else if (store != null)
            {
                storemember.IsVeryfying = true;
            }

            return storemember;
        }

        /// <summary>
        ///  創建店家並修改會員資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool CreateStore(StoreRegisterViewModel request)
        {
            var member = _repo.GetAll<Member>().First(x => x.MemberId == request.MemberId);
            member.StoreAuditStatus = 0;

            //創建店家
            var store = new Store()
            {
                Name = request.StoreName,
                Img = request.Img,
                KeyWord = request.KeyWord,
                //StoreCategoryId=1,
                Address = request.Address,
                Cellphone = request.PhoneNumber,
                CreateTime = DateTime.UtcNow.AddHours(8),
                IsDelete = false,
                MemberId = request.MemberId,
                StoreCategoryId = request.StoreCategoryId,
            };
            DbContext context = new HolyShongContext();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    _repo.Create<Store>(store);
                    _repo.Update<Member>(member);
                    _repo.SaveChange();
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string exString = ex.ToString();
                    return false;
                }
            }
        }

    }
}