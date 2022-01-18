using HolyShong.Services;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HolyShong.Controllers
{
    public class DeliverController : Controller
    {
        private readonly OrderService _orderService;
        public DeliverController()
        {
            _orderService = new OrderService();
        }
        // GET: Deliver
        [Authorize]
        public ActionResult Index()
        
        {
            var memberId = int.Parse(User.Identity.Name);
            //if(memberId == 0)
            //{
            //    return RedirectToAction("DeliverRegister", "Member");
            //}
            var result = _orderService.GetOrderForDeliver(memberId);
            return View(result);
        }

        /// <summary>
        /// 外送員切換上下線
        /// </summary>
        /// <param name="deliverConnectionVM"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeliverConnectionStatus(DeliverConnectionViewModel deliverConnectionVM)
        {
            var memberId = int.Parse(User.Identity.Name);
            deliverConnectionVM.memberId = memberId;
            var connectionResult = _orderService.SwitchDeliverConnection(deliverConnectionVM);

            return connectionResult;
        }

        /// <summary>
        /// 外送員訂單與物流狀況修改
        /// </summary>
        [HttpPost]
        public void OrderStateChange(OrderStatusViewModel OrderStatusVM)
        {
            var memberId = int.Parse(User.Identity.Name);
            OrderStatusVM.MemberId = memberId;
            _orderService.ChangeOrderState(OrderStatusVM);
        }

        public ActionResult DeliverOrderList()
        {
            var memberId = int.Parse(User.Identity.Name);
            var result = _orderService.GetOrderListByDeliver(memberId);
            return View(result);
        }



    }
}