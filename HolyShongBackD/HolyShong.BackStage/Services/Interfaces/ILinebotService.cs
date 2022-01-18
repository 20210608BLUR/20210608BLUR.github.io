using HolyShong.BackStage.ViewModels.Linebot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HolyShong.BackStage.Services.Interfaces
{
    public interface ILinebotService
    {
        LinebotViewModel SearchRestaurant(string description);

        //public string RestaurantToJson(LinebotViewModel entity);
    }
}
