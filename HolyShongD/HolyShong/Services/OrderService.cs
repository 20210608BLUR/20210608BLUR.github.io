using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using HolyShong.Models.HolyShongModel;
using System.Data.Entity;
using Newtonsoft.Json;

namespace HolyShong.Services
{
    public class OrderService
    {
        private readonly HolyShongRepository _repo;
        private readonly CartService _cartService;
        private readonly DiscountService _discountService;
        public OrderService()
        {
            _repo = new HolyShongRepository();
            _cartService = new CartService();
            _discountService = new DiscountService();
        }

        /// <summary>
        /// 成立訂單
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="checkoutVM"></param>
        /// <returns></returns>
        public OperationResult OrderCreate(int memberId, GetResultFromECPay EcPay)
        {
            //初始化OperationResult
            OperationResult result = new OperationResult();

            //轉換JSON
            var createOrderJson = EcPay.CustomField2.Replace("?", "\"");
            var checkOutInfo = JsonConvert.DeserializeObject<CreateOrderViewModel>(createOrderJson);

            //總額
            var cart = _repo.GetAll<Cart>().First(c => c.MemberId == memberId);
            var originMoney = _cartService.CalculateMoneyByCart(cart);
            //優惠金額
            var discountedPrice = _discountService.CalculateTotalWithDiscountMemberId(checkOutInfo.M,originMoney);
            var discountPrice = originMoney - discountedPrice;

            //產生order
            var findCart = _cartService.SearchCart(memberId);
            var findItems = _cartService.SearchItems(findCart);
            var findItemDetails = _cartService.SearchItemDetails(findItems);
            var findItemsGroup = findItems.Select(i => i.ProductId);
            var products = _repo.GetAll<Product>().Where(p => findItemsGroup.Contains(p.ProductId));
            var address = _repo.GetAll<Address>().First(a => a.MemberId == memberId && a.IsDefault == true);

            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    //加入新order
                    Order order = new Order()
                    {
                        MemberId = memberId,
                        StoreId = findCart.StroreId,
                        DeliveryFee = checkOutInfo.D,
                        Notes = checkOutInfo.N, 
                        DeliveryAddress = address.AddressDetail,
                        IsTablewares = checkOutInfo.T == "on" ? true : false,
                        IsPlasticbag = checkOutInfo.P == "on" ? true : false,
                        PaymentStatus = 1,
                        DeliverStatus = 0,
                        OrderStatus = 1,
                        MemberDiscountId = checkOutInfo.M,
                        CreateDate = DateTime.UtcNow.AddHours(8),
                        RequiredDate = DateTime.UtcNow.AddHours(8),
                        OrderStatusUpdateTime = DateTime.UtcNow.AddHours(8),
                        OrginalMoney = originMoney,
                        DiscountMoney = discountPrice
                    };
                    //先存才能拿到orderId
                    _repo.Create(order);
                    _repo.SaveChange();


                    foreach(var item in findItems)
                    {
                        var product = products.First(p=>p.ProductId == item.ProductId);
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            UnitPrice = product.UnitPrice,
                            Quantity = item.Quantity
                        };
                        _repo.Create(orderDetail);
                        _repo.SaveChange();

                        var findItemDetail = findItemDetails.Where(fid=>fid.ItemId == item.ItemId);
                        foreach (var detail in findItemDetail)
                        {
                            OrderDetailOption orderDetailOption = new OrderDetailOption()
                            {
                                OrderDetailId = orderDetail.OrderDetailId,
                                ProductOptionDetailId = detail.ProductOptionDetailId
                            };
                            _repo.Create(orderDetailOption);
                            _repo.SaveChange();
                        }
                    }

                    //刪除cart
                    _cartService.DeleteCart(findCart,findItems,findItemDetails);


                    //優惠卷新增
                    var discountMember = _repo.GetAll<DiscountMember>().FirstOrDefault(dm => dm.DiscountMemberId == checkOutInfo.M);
                    if(discountMember != null)
                    {
                        discountMember.IsUsed = true;
                        _repo.Update(discountMember);

                        var discountStore = _repo.GetAll<DiscountStore>().FirstOrDefault(ds => ds.DiscountId == discountMember.DiscountId);
                        if (discountStore != null)
                        {
                            discountStore.UsedNumber += 1;
                            _repo.Update(discountStore);
                        }
                    }

                    _repo.SaveChange();
                    result.IsSuccessful = true;
                    result.Result = order.OrderId;
                    tran.Commit();

                }
                catch (Exception ex)
                {
                    result.IsSuccessful = false;
                    result.Exception = ex.ToString();
                    tran.Rollback();
                }
            }
            return result;
        }

        /// <summary>
        /// 找最新訂單
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public int GetNewestOrderId(int memberId)
        {
            var orderId = _repo.GetAll<Order>().Where(o => o.MemberId == memberId).OrderByDescending(o => o.OrderId).First().OrderId;
            return orderId;
        }

        /// <summary>
        /// 歷史訂單透過orderId連接
        /// </summary>
        /// <returns></returns>
        public OrderDeliverViewModel GetOrder(int orderId)
        {

            OrderDeliverViewModel orderResult = new OrderDeliverViewModel()
            {
                OrderLists = new List<OrderList>()
            };

            var order = _repo.GetAll<Order>().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return orderResult;
            }

            //若還沒有外送員
            var deliver = _repo.GetAll<Deliver>().FirstOrDefault(d => d.DeliverId == order.DeliverId);
            if (deliver != null)
            {
                var member = _repo.GetAll<Member>().First(m => m.MemberId == deliver.MemberId);
                orderResult.DeliverName = member.LastName + member.FirstName;
                orderResult.DeliverImg = deliver.HeadshotImg;
            }
           
            var store = _repo.GetAll<Store>().First(s => s.StoreId == order.StoreId);
            var orderDetail = _repo.GetAll<OrderDetail>().Where(od => od.OrderId == order.OrderId);
            var product = _repo.GetAll<Product>().Where(p => orderDetail.Select(od => od.ProductId).Contains(p.ProductId));

            var productlist = orderDetail.Select(od => new OrderList
            {
                ProductName = product.FirstOrDefault(p => p.ProductId == od.ProductId).Name,
                ProductQuantity = od.Quantity
            }).ToList();

            var total = CalculateMoneyByOrder(order);

            orderResult.OrderStatus = order.OrderStatus;
            orderResult.CustomerAddress = order.DeliveryAddress;
            orderResult.CustomerNotes = order.Notes;
            orderResult.RestaurantName = store.Name;
            orderResult.RestaurantAddress = store.Address;
            orderResult.OrderLists = productlist;
            orderResult.Total = total;

            return orderResult;
        }

        /// <summary>
        /// 訂單總金額
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public decimal CalculateMoneyByOrder(Order order)
        {
            decimal totalPrice = 0;
            decimal addPrice = 0;

            var orderDetails = _repo.GetAll<OrderDetail>().Where(od => od.OrderId == order.OrderId);
            var orderDetailOptions = _repo.GetAll<OrderDetailOption>().Where(odo => orderDetails.Select(od => od.OrderDetailId).Contains(odo.OrderDetailId));
            var store = _repo.GetAll<Store>().First(s => s.StoreId == order.StoreId);
            var products = _repo.GetAll<Product>().Where(p => orderDetails.Select(od => od.ProductId).Contains(p.ProductId));
            var productOptions = _repo.GetAll<ProductOption>().Where(po => products.Select(p => p.ProductId).Contains(po.ProductId));
            var productOptionDetails = _repo.GetAll<ProductOptionDetail>().Where(pod => productOptions.Select(po => po.ProductOptionId).Contains(pod.ProductOptionId));


            foreach (var orderDetail in orderDetails)
            {
                var price = products.First(p => p.ProductId == orderDetail.ProductId).UnitPrice;
                var orderDetailOption = orderDetailOptions.Where(odo => odo.OrderDetailId == orderDetail.OrderDetailId);
                if (orderDetailOption == null) return totalPrice += orderDetail.Quantity * price;

                foreach (var option in orderDetailOption)
                {
                    var add = productOptionDetails.First(pod => pod.ProductOptionDetailId == option.ProductOptionDetailId).AddPrice ?? 0;
                    addPrice += add;
                }
                totalPrice += orderDetail.Quantity * (price + addPrice);
                addPrice = 0;
            }
            return totalPrice;
        }

        /// <summary>
        /// 查詢會員歷史訂單
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<OrderListViewModel> GetOrderByMemberId(int memberId)
        {
            List<OrderListViewModel> result = new List<OrderListViewModel>();

            var member = _repo.GetAll<Member>().FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
            {
                return new List<OrderListViewModel>();
            }

            var orders = _repo.GetAll<Order>().Where(o => o.MemberId == member.MemberId).OrderByDescending(o => o.OrderId);
            var stores = _repo.GetAll<Store>().Where(s => orders.Select(o => o.StoreId).Contains(s.StoreId));
            var orderDetails = _repo.GetAll<OrderDetail>().Where(od => orders.Select(o => o.OrderId).Contains(od.OrderId));
            var orderOptionDetails = _repo.GetAll<OrderDetailOption>().Where(odo => orderDetails.Select(od => od.OrderDetailId).Contains(odo.OrderDetailId));
            var products = _repo.GetAll<Product>().Where(p => orderDetails.Select(od => od.ProductId).Contains(p.ProductId));
            var productOptionDetails = _repo.GetAll<ProductOptionDetail>().Where(pod => orderOptionDetails.Select(odo => odo.ProductOptionDetailId).Contains(pod.ProductOptionDetailId));
            var productOptions = _repo.GetAll<ProductOption>().Where(po => productOptionDetails.Select(pod => pod.ProductOptionId).Contains(po.ProductOptionId));

            foreach (var o in orders)
            {
                var productNumber = 0;
                //ProductName 需要orderDetail Product productOptionDetail
                var orderDetail = orderDetails.Where(od => od.OrderId == o.OrderId);
                var orderOptionDetail = orderOptionDetails.Where(ood => orderDetail.Select(od => od.OrderDetailId).Contains(ood.OrderDetailId));
                var productOptionDetail = productOptionDetails.Where(pod => orderOptionDetail.Select(ood => ood.ProductOptionDetailId).Contains(pod.ProductOptionDetailId));

                List<OrderProduct> op = new List<OrderProduct>();
                var product = products.Where(p => orderDetail.Select(od => od.ProductId).Contains(p.ProductId));

                foreach (var p in product)
                {
                    var odByproduct = orderDetail.FirstOrDefault(od => od.ProductId == p.ProductId);
                    var oOptionDetail = orderOptionDetails.Where(ood => ood.OrderDetailId == odByproduct.OrderDetailId);
                    var pOptionDetail = productOptionDetails.Where(pod => oOptionDetail.Select(ood => ood.ProductOptionDetailId).Contains(pod.ProductOptionDetailId));
                    var pOptionDetailName = pOptionDetail.Select(pod => pod.Name);
                    var pOptionDetailString = String.Join("．", pOptionDetailName);

                    var temp = new OrderProduct()
                    {
                        ProductName = p.Name,
                        ProductPrice = odByproduct.UnitPrice,
                        ProductQuantity = odByproduct.Quantity,
                        OrderOptions = pOptionDetailString
                    };
                    op.Add(temp);

                }
                foreach (var p in op)
                {
                    productNumber += p.ProductQuantity;
                }


                decimal addPrice = productOptionDetail.Sum(pod => pod.AddPrice).HasValue ? productOptionDetail.Sum(pod => pod.AddPrice).Value : 0;
                var tempOrder = new OrderListViewModel
                {
                    OrderId = o.OrderId,
                    DeliverDate = o.RequiredDate == null ? o.CreateDate : o.RequiredDate,
                    RestaurantId = stores.First(s => s.StoreId == o.StoreId).StoreId,
                    RestaurantName = stores.First(s => s.StoreId == o.StoreId).Name,
                    RestaurantImg = stores.First(s => s.StoreId == o.StoreId).Img,
                    ProductLists = op,
                    ProductCount = productNumber,
                    OrderStatus = o.OrderStatus,
                    Total = o.OrginalMoney + o.DeliveryFee - (o.DiscountMoney??0)
                };
                result.Add(tempOrder);
            }
            return result;
        }



        //外送員

        /// <summary>
        /// 外送員歷史訂單
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<OrderListViewModel> GetOrderListByDeliver(int memberId)
        {
            List<OrderListViewModel> result = new List<OrderListViewModel>();

            var deliver = _repo.GetAll<Deliver>().FirstOrDefault(d => d.MemberId == memberId);
            if (deliver == null)
            {
                return new List<OrderListViewModel>();
            }

            var orders = _repo.GetAll<Order>().Where(o => o.DeliverId == deliver.DeliverId);
            var stores = _repo.GetAll<Store>().Where(s => orders.Select(o => o.StoreId).Contains(s.StoreId));
            //todo tolist
            var orderDetails = _repo.GetAll<OrderDetail>().Where(od => orders.Select(o => o.OrderId).Contains(od.OrderId));
            //todo 用新的變數取代orderDetails.Select(od => od.OrderDetailId)
            var orderOptionDetails = _repo.GetAll<OrderDetailOption>().Where(odo => orderDetails.Select(od => od.OrderDetailId).Contains(odo.OrderDetailId));
            var products = _repo.GetAll<Product>().Where(p => orderDetails.Select(od => od.ProductId).Contains(p.ProductId));
            var productOptionDetails = _repo.GetAll<ProductOptionDetail>().Where(pod => orderOptionDetails.Select(odo => odo.ProductOptionDetailId).Contains(pod.ProductOptionDetailId));
            var productOptions = _repo.GetAll<ProductOption>().Where(po => productOptionDetails.Select(pod => pod.ProductOptionId).Contains(po.ProductOptionId));
            //todo 處理效能
            foreach (var o in orders)
            {
                var productNumber = 0;
                //ProductName 需要orderDetail Product productOptionDetail
                var orderDetail = orderDetails.Where(od => od.OrderId == o.OrderId);
                var orderOptionDetail = orderOptionDetails.Where(ood => orderDetail.Select(od => od.OrderDetailId).Contains(ood.OrderDetailId));
                var productOptionDetail = productOptionDetails.Where(pod => orderOptionDetail.Select(ood => ood.ProductOptionDetailId).Contains(pod.ProductOptionDetailId));
                var productOptionDetailName = productOptionDetail.Select(pod => pod.Name);
                var productOptionDetailString = String.Join("．", productOptionDetailName);
                var product = products.Where(p => orderDetail.Select(od => od.ProductId).Contains(p.ProductId)).
                    Select(p => new OrderProduct
                    {
                        ProductName = p.Name,
                        //todo 要處理null問題
                        ProductPrice = orderDetail.FirstOrDefault(od => od.ProductId == p.ProductId).UnitPrice,
                        //todo 要處理null問題
                        ProductQuantity = orderDetail.FirstOrDefault(od => od.ProductId == p.ProductId).Quantity,
                        OrderOptions = productOptionDetailString,
                        
                    }).ToList();
                foreach (var p in product)
                {
                    productNumber += p.ProductQuantity;
                }

                var amount = orderDetail.Sum(od => od.Quantity * od.UnitPrice);
                decimal addPrice = productOptionDetail.Sum(pod => pod.AddPrice).HasValue ? productOptionDetail.Sum(pod => pod.AddPrice).Value : 0;
                var tempOrder = new OrderListViewModel
                {
                    OrderId = o.OrderId,
                    DeliverDate = o.RequiredDate == null ? o.CreateDate : o.RequiredDate,
                    //todo 要處理null問題
                    RestaurantId = stores.FirstOrDefault(s => s.StoreId == o.StoreId).StoreId,
                    //todo 要處理null問題
                    RestaurantName = stores.FirstOrDefault(s => s.StoreId == o.StoreId).Name,
                    //todo 要處理null問題
                    RestaurantImg = stores.FirstOrDefault(s => s.StoreId == o.StoreId).Img,
                    ProductLists = product,
                    ProductCount = productNumber,
                    OrderStatus = o.OrderStatus,
                    DeliverFee = 30,
                };
                result.Add(tempOrder);
            }
            return result;
        }


        /// <summary>
        /// 外送員上下線切換
        /// </summary>
        /// <param name="connectionStatus"></param>
        /// <returns></returns>
        public bool SwitchDeliverConnection(DeliverConnectionViewModel deliverConnectionVM)
        {
            //透過會員狀態關連到deliverId
            //todo 要處理null問題
            var deliverId = _repo.GetAll<Deliver>().FirstOrDefault(d=>d.MemberId == deliverConnectionVM.memberId).DeliverId;

            //找出外送員有沒有在外送，有的話駁回切換下線
            //todo 要處理null問題
            var deliverStatus = _repo.GetAll<Deliver>().FirstOrDefault(d => d.DeliverId == deliverId).isDelivering;
            if (deliverStatus == true)
            {
                deliverConnectionVM.isOnline = !deliverConnectionVM.isOnline;
                return deliverConnectionVM.isOnline;
            }

            //其餘儲存狀態修改
            //VM->DM
            var deliverInfo = _repo.GetAll<Deliver>().FirstOrDefault(d => d.DeliverId == deliverId);

            deliverInfo.isOnline = deliverConnectionVM.isOnline;

            //update+savechange
            _repo.Update(deliverInfo);
            _repo.SaveChange();

            return deliverConnectionVM.isOnline;
        }

        /// <summary>
        /// 抓外送員判斷資料庫中上下線
        /// </summary>
        /// <param name="memberEmail"></param>
        /// <returns></returns>
        public Deliver GetDeliverInfo(string inputMemberId)
        {
            //var memberId = _repo.GetAll<Member>().First(m=>m.Email == memberEmail).MemberId;
            int memberId = int.Parse(inputMemberId);
            var deliver = _repo.GetAll<Deliver>().First(d => d.MemberId == memberId);

            return deliver;
        }

        /// <summary>
        /// 外送員外送畫面
        /// </summary>
        /// <returns></returns>
        public DeliverViewModel GetOrderForDeliver(int memberId)
        {
            DeliverViewModel result = new DeliverViewModel() { OrderProducts = new List<OrderProducts>() };

            //透過帳號登入抓到這個人的memberId & DeliverId
            var member = _repo.GetAll<Member>().FirstOrDefault(m => m.MemberId == memberId);
            var deliver = _repo.GetAll<Deliver>().FirstOrDefault(d => d.MemberId == memberId);


            //透過deliverId 去抓出他的外送訂單中，運送中的單(orderstatus = 4 && deliverStatus = 1)，取orderId
            var order = _repo.GetAll<Order>().Where(o => o.DeliverId ==deliver.DeliverId).FirstOrDefault(o => o.OrderStatus == 4 || o.OrderStatus == 5);
            if (order == null)
            {
                return result;
            }

            //透過orderId取得訂單其他資訊
            var store = _repo.GetAll<Store>().FirstOrDefault(s => s.StoreId == order.StoreId);
            var orderDetails = _repo.GetAll<OrderDetail>().Where(od => od.OrderId == order.OrderId);
            var products = _repo.GetAll<Product>().Where(p => orderDetails.Select(od => od.ProductId).Contains(p.ProductId));

            result.OrderCode = "EAT" + order.OrderId.ToString().PadLeft(5, '0');
            result.OrderStatus = order.OrderStatus;
            result.CustomerName = member.LastName + member.FirstName;
            result.CustomerAddress = order.DeliveryAddress;
            result.CustormerNotes = order.Notes;
            result.RestaurantName = store.Name;
            result.RestaurantAddress = store.Address;
            result.OrderProducts = orderDetails.Select(od => new OrderProducts
            {
                ProductName = products.FirstOrDefault(p => p.ProductId == od.ProductId).Name,
                ProductQuantity = od.Quantity

            }).ToList();
            return result;
        }

        /// <summary>
        /// 外送員訂單狀態與物流狀態改變
        /// </summary>
        public OperationResult ChangeOrderState(OrderStatusViewModel OrderStatusVM)
        {
            OperationResult result = new OperationResult();

            //抓memberId
            var memberId = OrderStatusVM.MemberId;

            //VM中分析他的orderID
            var orderId = int.Parse(OrderStatusVM.OrderCode.Substring(3, 5));
            //先抓到訂單
            var order = _repo.GetAll<Order>().FirstOrDefault(o => o.OrderId == orderId);

            //交易
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                //todo orderstatus做成enum=>另開資料夾
                try
                {
                    //傳入訂單狀態判斷
                    //餐廳完成訂單，安排外送員
                    if (OrderStatusVM.OrderStatus == 4)
                    {
                        //var notSelf = _repo.GetAll<Deliver>().Where(d => d.MemberId != memberId);
                        var freeDeliver = _repo.GetAll<Deliver>().Where(d => d.isOnline == true && d.isDelivering == false).OrderBy(d => d.DeliverId).First();
                        order.DeliverId = freeDeliver.DeliverId;
                        //外送員改成送貨中
                        freeDeliver.isDelivering = true;
                        order.OrderStatus = 4;

                        //update
                        _repo.Update(freeDeliver);
                    }
                    //開始外送
                    else if(OrderStatusVM.OrderStatus == 5)
                    {
                        //orderstate改變
                        order.OrderStatus = 5;
                        //deliverstatus改變
                        order.DeliverStatus = 2;
                    }
                    //完成外送
                    else if(OrderStatusVM.OrderStatus == 6)
                    {
                        order.OrderStatus = 6;
                        order.DeliverStatus = 3;

                        var deliver = _repo.GetAll<Deliver>().FirstOrDefault(d=>d.DeliverId == order.DeliverId);
                        deliver.isDelivering = false;
                        //外送員改成非外送中
                        _repo.Update(deliver);
                        _repo.SaveChange();
                    }
                    result.IsSuccessful = true;
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    result.IsSuccessful = false;
                    result.Exception = ex.ToString();
                    tran.Rollback();
                }
            }
            //update
            _repo.Update(order);
            _repo.SaveChange();
            return result;

        }

    }
}


