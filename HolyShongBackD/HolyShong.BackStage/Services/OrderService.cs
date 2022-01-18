using HolyShong.BackStage.Services.Interfaces;
using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbRepository _repo;
        public OrderService(IDbRepository repo)
        {
            _repo = repo;
        }

        ///// <summary>
        ///// 所有訂單
        ///// </summary>
        ///// <returns></returns>
        //public List<OrderResponseViewModel> GetAllOrders()
        //{
        //    var repoOrders = _repo.GetAll<Order>();
        //    var stores = _repo.GetAll<Store>();
        //    var discounts = _repo.GetAll<Discount>();
        //    var discountMembers = _repo.GetAll<DiscountMember>();
        //    //
        //    var repoOrderDetails = _repo.GetAll<OrderDetail>().ToList();
        //    var repoOrderDetailOptions = _repo.GetAll<OrderDetailOption>();
        //    var ordersVM = new List<OrderResponseViewModel>();
        //    var productOptionDetail = _repo.GetAll<ProductOptionDetail>();
        //    foreach (var repoOrder in repoOrders)
        //    {
        //        //優惠券名稱
        //        var discountName = "未使用";
        //        if (repoOrder.MemberDiscountId != null)
        //        {
        //            discountName = discounts.First(x => discountMembers.First(y => y.DiscountMemberId == repoOrder.MemberDiscountId).DiscountId == x.DiscountId)
        //                .DisplayName;
        //        }

        //        var temp = new OrderResponseViewModel
        //        {
        //            OrderId = repoOrder.OrderId,
        //            MemberId = repoOrder.MemberId,
        //            Address = repoOrder.DeliveryAddress,
        //            CreateTime = repoOrder.CreateDate,
        //            DeliverFee = repoOrder.DeliveryFee,
        //            Tips = repoOrder.Tips,
        //            DeliverId = repoOrder.DeliverId,
        //            DeliverStatus = repoOrder.DeliverStatus,
        //            IsPlasticbag = repoOrder.IsPlasticbag,
        //            IstableWares = repoOrder.IsPlasticbag,
        //            MemberDiscountId = repoOrder.MemberDiscountId,
        //            DiscountName = discountName,
        //            DiscountMoney = repoOrder.DiscountMoney,
        //            Note = repoOrder.Notes,
        //            OrderPrice = repoOrder.OrginalMoney,
        //            OrderStatus = repoOrder.OrderStatus,
        //            PaymentStatus = repoOrder.PaymentStatus,
        //            Score = repoOrder.Score,
        //            StoreId = repoOrder.StoreId,
        //            StoreName = stores.First(x => x.StoreId == repoOrder.StoreId).Name,
        //            RequiredDate = repoOrder.RequiredDate,
        //            OrderDetailResponses = repoOrderDetails.Where(od => od.OrderId == repoOrder.OrderId)
        //             .Select(od => new OrderDetailResponse()
        //             {
        //                 OrderDetailId = od.OrderDetailId,
        //                 OrderID = od.OrderId,
        //                 ProductId = od.ProductId,
        //                 Quantity = od.Quantity,
        //                 UnitPrice = od.UnitPrice,
        //                 OrderDetailOptionResponses = repoOrderDetailOptions.Where(odOption => odOption.OrderDetailOptionId == od.OrderDetailId)
        //                     .Select(odOption => new OrderDetailOptionResponse()
        //                     {
        //                         OrderOptionIdDetailId = odOption.OrderDetailOptionId,
        //                         OrderDeetailId = odOption.OrderDetailId,
        //                         ProductOptionDetailId = odOption.ProductOptionDetailId,
        //                         ProductOptionDetailName = productOptionDetail.First(x => x.ProductOptionDetailId == odOption.ProductOptionDetailId).Name,
        //                         Addprice = productOptionDetail.First(x => x.ProductOptionDetailId == odOption.ProductOptionDetailId).AddPrice,
        //                     })
        //             })
        //        };

        //        ordersVM.Add(temp);
        //    }

        //    return ordersVM;
        //}

        ///// <summary>
        ///// 待出餐訂單
        ///// orderStatus=1或2
        ///// </summary>
        ///// <returns></returns>
        //public List<OrderResponseViewModel> GetPreparingOrder()
        //{
        //    var allOrders = GetAllOrders();
        //    var preparingORders = allOrders.Where(x => x.OrderStatus == 1 || x.OrderStatus == 2).ToList();
        //    return preparingORders;
        //}

        ///// <summary>
        ///// 歷史訂單訂單
        ///// orderStatus=3~6其一
        ///// </summary>
        ///// <returns></returns>
        //public List<OrderResponseViewModel> GetHistoryORders()
        //{
        //    var allOrders = GetAllOrders();
        //    var historyOrders = allOrders.Where(x => x.OrderStatus == 3 || x.OrderStatus == 4 || x.OrderStatus == 5 || x.OrderStatus == 6).ToList();
        //    return historyOrders;
        //}

        
        /// <summary>
        /// 找出符合訂單狀態的訂單
        /// 重購
        /// </summary>
        /// <param name="orderStatuses"></param>
        /// <returns></returns>
        public List<OrderResponseViewModel> GetOrdersByStatus(List<int> orderStatuses)
        {
            var ordersVM = new List<OrderResponseViewModel>();
            //所有OrderStatus=參數orderStatuses的資料
            var repoOrders = _repo.GetAll<Order>().Where(x => orderStatuses.Select(y => y).Contains(x.OrderStatus));
            var stores = _repo.GetAll<Store>();
            var discounts = _repo.GetAll<Discount>();
            var discountMembers = _repo.GetAll<DiscountMember>();
            var repoOrderDetails = _repo.GetAll<OrderDetail>().ToList();
            var repoProducts = _repo.GetAll<Product>();
            var repoOrderDetailOptions = _repo.GetAll<OrderDetailOption>();
            var repoProductOptionDetails = _repo.GetAll<ProductOptionDetail>();
            foreach (var repoOrder in repoOrders)
            {
                //優惠券名稱
                var discountName = "未使用";
                if (repoOrder.MemberDiscountId != null && repoOrder.MemberDiscountId !=0)
                {
                    discountName = discounts.First(x => discountMembers.First(y => y.DiscountMemberId == repoOrder.MemberDiscountId).DiscountId == x.DiscountId)
                        .DisplayName;
                }

                var temp = new OrderResponseViewModel
                {
                    OrderId = repoOrder.OrderId,
                    MemberId = repoOrder.MemberId,
                    Address = repoOrder.DeliveryAddress,
                    CreateTime = repoOrder.CreateDate,
                    DeliverFee = repoOrder.DeliveryFee,
                    Tips = repoOrder.Tips,
                    DeliverId = repoOrder.DeliverId,
                    DeliverStatus = repoOrder.DeliverStatus,
                    IsPlasticbag = repoOrder.IsPlasticbag,
                    IstableWares = repoOrder.IsPlasticbag,
                    MemberDiscountId = repoOrder.MemberDiscountId,
                    DiscountName = discountName,
                    DiscountMoney = repoOrder.DiscountMoney,
                    Note = repoOrder.Notes,
                    OrderPrice = repoOrder.OrginalMoney,
                    OrderStatus = repoOrder.OrderStatus,
                    PaymentStatus = repoOrder.PaymentStatus,
                    Score = repoOrder.Score,
                    StoreId = repoOrder.StoreId,
                    //新增
                    NotCheck = true,
                    StoreName = stores.First(x => x.StoreId == repoOrder.StoreId).Name,
                    RequiredDate = repoOrder.RequiredDate,
                    OrderDetailResponses = repoOrderDetails.Where(od => od.OrderId == repoOrder.OrderId)
                     .Select(od => new OrderDetailResponse()
                     {
                         OrderDetailId = od.OrderDetailId,
                         OrderID = od.OrderId,
                         ProductId = od.ProductId,
                         ProductName = repoProducts.First(x => x.ProductId == od.ProductId).Name,
                         Quantity = od.Quantity,
                         UnitPrice = od.UnitPrice,
                         OrderDetailOptionResponses = repoOrderDetailOptions.Where(odOption => odOption.OrderDetailId == od.OrderDetailId)
                             .Select(odOption => new OrderDetailOptionResponse()
                             {
                                 OrderOptionIdDetailId = odOption.OrderDetailOptionId,
                                 OrderDeetailId = odOption.OrderDetailId,
                                 ProductOptionDetailId = odOption.ProductOptionDetailId,
                                 ProductOptionDetailName = repoProductOptionDetails.First(x => x.ProductOptionDetailId == odOption.ProductOptionDetailId).Name,
                                 Addprice = repoProductOptionDetails.First(x => x.ProductOptionDetailId == odOption.ProductOptionDetailId).AddPrice,
                             }).ToList()
                     }).ToList()
                };
                if (temp.OrderStatus == 1) temp.OrderStatusName = "已付款";
                if (temp.OrderStatus == 2) temp.OrderStatusName = "準備中";
                if (temp.OrderStatus == 3) temp.OrderStatusName = "餐點完成";
                if (temp.OrderStatus == 4) temp.OrderStatusName = "等待配送";
                if (temp.OrderStatus == 5) temp.OrderStatusName = "配送中";
                if (temp.OrderStatus == 6) temp.OrderStatusName = "已配送";
                ordersVM.Add(temp);
            }

            return ordersVM;
        }

        /// <summary>
        /// 更新訂單狀態
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string UpdateOrder(OrderStatusResponseViewModel request)
        {

            var order = _repo.GetAll<Order>().First(x => x.OrderId == request.OrderId);
            order.OrderStatus = request.OrderStatus;
            order.OrderStatusUpdateTime = DateTime.UtcNow.AddHours(8);
            var result = "";
            using (var transaction = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    _repo.Update<Order>(order);
                    _repo.Save();
                    transaction.Commit();
                    result = "success";
                }
                catch (Exception ex)
                {
                    result = ex.ToString();
                    transaction.Rollback();
                }

                return result;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderStatuses"></param>
        /// <returns></returns>
        //public List<OrderResponseViewModel> GetOrdersByStoreId(List<int> orderStatuses)
        //{

        //}


    }

}
