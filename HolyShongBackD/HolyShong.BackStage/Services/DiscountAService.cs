using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.ViewModels;
using HolyShong.BackStage.ViewModels.Discount;

namespace HolyShong.BackStage.Services
{
    public class DiscountAService : IDiscountAService
    {
        private readonly IDbRepository _repo;
        public DiscountAService(IDbRepository repo)
        {
            _repo = repo;
        }

        public void CreateDiscount(CreateDiscountViewModel request)
        {
            var entity = new Discount()
            {
                DiscountCode = request.DiscountCode,
                DisplayName = request.DisplayName,
                Type =request.Type,
                Amount = request.Amount,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                UseLimit = null,
                Title = request.Title,
                Contents = request.Contents,
                Img = request.Img,
                IsAllStore = request.IsAllStore
            };
            _repo.Create(entity);
            _repo.Save();

        }
        public void UpdateDiscount(UpdateDiscountViewModel request)
        {
            var target = _repo.GetAll<Discount>().First(x => x.DiscountId == request.DiscountId);
          
            target.DiscountCode = request.DiscountCode;
            target.Type = request.Type;
            target.Amount = request.Amount;

            _repo.Update(target);
            _repo.Save();
        }
        public void DeleteDiscount(DeleteDiscountViewModel request)
        {
            var target = _repo.GetAll<Discount>().FirstOrDefault(x => x.DiscountId == request.DiscountId);
            _repo.Delete(target);
            _repo.Save();
        }

        public List<DiscountResponseViewModel> GetAllDiscount()
        {
            var discountResult = new List<DiscountResponseViewModel>();
            discountResult = _repo.GetAll<Discount>().ToList().Select(d => new DiscountResponseViewModel(){

                DiscountId = d.DiscountId,
                DiscountCode = d.DiscountCode,
                DisplayName = d.DisplayName,
                Type = d.Type,
                Amount = d.Amount,
                StartTime = d.StartTime?.ToString("yyyy-MM-dd"),
                EndTime = d.EndTime?.ToString("yyyy-MM-dd"),
                UseLimit = d.UseLimit,
                Title = d.Title,
                Contents = d.Contents,
                Img = d.Img,
                IsAllStore = d.IsAllStore

            }).ToList();


            return discountResult;
        

        }

        public List<DiscountResponseViewModel> GetEffectiveDiscount()
        {
            var effectiveDiscount = new List<DiscountResponseViewModel>();
            var now = DateTime.UtcNow.AddHours(8);
            effectiveDiscount = _repo.GetAll<Discount>().Where(x => x.EndTime >= now).ToList().Select(x => new DiscountResponseViewModel
            {
                DiscountCode = x.DiscountCode,
                DiscountId = x.DiscountId,
                DisplayName = x.DisplayName,
                StartTime = x.StartTime?.ToString("yyyy-MM-dd"),
                EndTime = x.EndTime?.ToString("yyyy-MM-dd"),
                Type = x.Type,
                Amount = x.Amount,
                Contents = x.Contents,
                Img = x.Img,
                IsAllStore = x.IsAllStore,
                Title = x.Title,
                UseLimit = x.UseLimit



            }).ToList();
            return effectiveDiscount;
        }

        public List<DiscountResponseViewModel> GetInvalidDiscount()
        {
            var invalidDiscount = new List<DiscountResponseViewModel>();
            var now = DateTime.UtcNow.AddHours(8);
            invalidDiscount = _repo.GetAll<Discount>().Where(x => x.EndTime < now).ToList().Select(x => new DiscountResponseViewModel
            {
                DiscountCode = x.DiscountCode,
                DisplayName = x.DisplayName,
                StartTime = x.StartTime?.ToString("yyyy-MM-dd"),
                EndTime = x.EndTime?.ToString("yyyy-MM-dd"),
                Amount = x.Amount,
                DiscountId = x.DiscountId,
                Contents = x.Contents,
                Img = x.Img,
                IsAllStore = x.IsAllStore,
                Title = x.Title,
                Type = x.Type,
                UseLimit = x.UseLimit
            }).ToList();
            return invalidDiscount;
        }
    }
}
