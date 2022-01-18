using HolyShong.BackStage.ViewModels.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services.Interfaces
{
    public interface IDiscountService
    {
        List<DiscountResponseViewModel> GetAllDiscount();
    }
}
