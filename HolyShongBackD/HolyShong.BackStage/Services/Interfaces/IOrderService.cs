using HolyShong.BackStage.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderResponseViewModel> GetOrdersByStatus(List<int> orderStatuses);
        string UpdateOrder(OrderStatusResponseViewModel request);

    }
}
