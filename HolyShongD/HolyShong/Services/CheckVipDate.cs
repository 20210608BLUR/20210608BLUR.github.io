using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.Repositories;
using HolyShong.Models.HolyShongModel;
using System.Data.Entity;

namespace HolyShong.Services
{
    public class CheckVipDate
    {
        private readonly HolyShongRepository _holyShongRepository;
        public CheckVipDate()
        {
            _holyShongRepository = new HolyShongRepository();
        }
        public void CheckVip(int id)
        {
            var member = _holyShongRepository.GetAll<Rank>().OrderByDescending(x => x.EndTime).FirstOrDefault(x => x.MemberId == id);
            if (member != null && member.EndTime != null && member.EndTime.Value < DateTime.UtcNow.AddHours(8))
            {
                member.IsPrimary = false;
                using (var transaction = _holyShongRepository.Context.Database.BeginTransaction())
                    try
                    {
                        _holyShongRepository.Update<Rank>(member);
                        _holyShongRepository.SaveChange();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
            }
        }
    }
}