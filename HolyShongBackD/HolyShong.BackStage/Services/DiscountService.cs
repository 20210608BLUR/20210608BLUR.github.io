using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDbRepository _repo;

        public DiscountService(IDbRepository repo)
        {
            _repo = repo;
        }

        public List<DiscountResponseViewModel> GetAllDiscount()
        {
            var discountResult = new List<DiscountResponseViewModel>();
            discountResult = _repo.GetAll<Discount>().ToList().Select(d => new DiscountResponseViewModel()
            {
                DiscountId = d.DiscountId,
                DiscountCode = d.DiscountCode,
                DisplayName = d.DisplayName,
                Amount = d.Amount,
                Contents = d.Contents,
                StartTime = d.StartTime?.ToString("yyyy-MM-dd"),
                EndTime = d.EndTime?.ToString("yyyy-MM-dd"),
                Img = d.Img,
                IsAllStore = d.IsAllStore,
                Title = d.Title,
                Type = d.Type,
                UseLimit = d.UseLimit
            }).ToList();

            return discountResult;
        }
    }
}
