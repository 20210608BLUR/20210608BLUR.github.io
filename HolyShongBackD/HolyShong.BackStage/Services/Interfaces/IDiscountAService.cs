using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolyShong.BackStage.ViewModels.Discount;

namespace HolyShong.BackStage.Services.Interfaces
{
    public interface IDiscountAService
    {
        List<DiscountResponseViewModel> GetAllDiscount();
        List<DiscountResponseViewModel> GetEffectiveDiscount();
        List<DiscountResponseViewModel> GetInvalidDiscount();
        void CreateDiscount(CreateDiscountViewModel request);
        void UpdateDiscount(UpdateDiscountViewModel request);
        void DeleteDiscount(DeleteDiscountViewModel request);
    }
}
