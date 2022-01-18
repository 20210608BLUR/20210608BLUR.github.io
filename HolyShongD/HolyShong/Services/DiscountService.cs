using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HolyShong.Services
{
    public class DiscountService
    {
        private readonly HolyShongRepository _repo;
        public DiscountService()
        {
            _repo = new HolyShongRepository();
        }

        /// <summary>
        /// 計算優惠後購物車總額
        /// </summary>
        /// <param name="discountMemberId"></param>
        /// <param name="totalPrice"></param>
        /// <returns></returns>
        public decimal CalculateTotalWithDiscountMemberId(int discountMemberId, decimal totalPrice)
        {
            if (discountMemberId == 0)
            {
                return totalPrice;
            }
            //找到優惠卷種類
            var discountMember = _repo.GetAll<DiscountMember>().First(dm => dm.DiscountMemberId == discountMemberId);
            var discount = _repo.GetAll<Discount>().First(d=>d.DiscountId == discountMember.DiscountId);
            var discountType = discount.Type;
            var discountAmount = discount.Amount;

            //折趴數
            if (discountType == 1)
            {
                return totalPrice -(Math.Round(totalPrice * (1 - discountAmount)));
            }
            //折金額
            else
            {
                return totalPrice - discountAmount;
            }
        }


        /// <summary>
        /// 漢堡處領用優惠卷
        /// </summary>
        /// <returns></returns>
        public string AcquireDiscount(string discountCode, int memberId)
        {
            if(discountCode == "")
            {
                return "不可為空白";
            }
            //找到此優惠卷ID，且仍在效期
            discountCode = discountCode.Trim();
            var nowDateTime = DateTime.UtcNow.AddHours(8);
            //找不到優惠卷
            var discount = _repo.GetAll<Discount>().FirstOrDefault(d => d.DiscountCode == discountCode);
            if(discount == null)
            {
                return "折扣碼錯誤，找不到優惠卷";
            }
            var isExpired = discount.EndTime >= nowDateTime;
            if (isExpired == false)
            {
                return "優惠卷已過期";
            }

            //找到優惠卷
            //判斷此會員有無領用過
            var haveDiscount = _repo.GetAll<DiscountMember>().FirstOrDefault(dm=>dm.MemberId == memberId && dm.DiscountId == discount.DiscountId);
            //有，不可領
            if (haveDiscount != null)
            {
                return "優惠卷已領用";
            }

            //無，領用
            using (var tran = _repo.Context.Database.BeginTransaction())
            {
                try
                {
                    //沒有，加入資料庫(DiscountMember && Discount)
                    DiscountMember discountMember = new DiscountMember()
                    {
                        DiscountId = discount.DiscountId,
                        MemberId = memberId,
                        IsUsed = false
                    };

                    //Discount 確認還有沒有額度，有要減一
                    var findDiscount = _repo.GetAll<Discount>().First(d => d.DiscountId == discount.DiscountId);
                    if (discount.UseLimit > 0)
                    {
                        discount.UseLimit -= 1;
                        _repo.Update(discount);
                    }
                    else if (discount.UseLimit == 0)
                    {
                        return "優惠卷被搶完了喔~";
                    }
                    _repo.Create(discountMember);
                    _repo.SaveChange();
                    tran.Commit();

                    return "完成新增";
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return ex.ToString();
                }
            }

        }

        /// <summary>
        /// 會員的所有有效優惠卷
        /// </summary>
        /// <returns></returns>
        public List<DiscountViewModel> GetDiscountByMemberId(int memberId, int storeId)
        {
            List<DiscountViewModel> discountVM = new List<DiscountViewModel>();

            var memberDiscounts = _repo.GetAll<DiscountMember>().Where(dm => dm.MemberId == memberId && dm.IsUsed == false);
            if(memberDiscounts == null)
            {
                return discountVM;
            }

            //找出還有效的discount
            var validDiscount = _repo.GetAll<Discount>().Where(d => memberDiscounts.Select(md => md.DiscountId).Contains(d.DiscountId)).Where(md => md.EndTime >= DateTime.Now);

            //判斷是否有店家
            if(storeId != 0)
            {
                //判斷店家是否能用
                var storeIsDiscount = _repo.GetAll<DiscountStore>().FirstOrDefault(ds => ds.StoreId == storeId);

                //不能則傳回有領用的通用優惠
                if(storeIsDiscount == null)
                {
                    validDiscount = validDiscount.Where(d => d.isAllStore == true);
                }
                //可以，則傳回通用&店家用優惠
                else
                {
                    //找到discountStore 有storeId的優惠卷
                    var storeDiscount = _repo.GetAll<DiscountStore>().Where(ds => ds.StoreId == storeId);
                    //篩出包含StoreId的優惠卷
                    validDiscount = validDiscount.Where(d => d.isAllStore == true || storeDiscount.Select(sd=> sd.DiscountId).Contains(d.DiscountId));
                }
            }

            discountVM = validDiscount.Select(d => new DiscountViewModel()
            {
                DiscountMemberId = memberDiscounts.FirstOrDefault(md => md.DiscountId == d.DiscountId).DiscountMemberId,
                DiscountName = d.DisplayName,
                EndTime = d.EndTime.Value,
                DiscountAmount = d.Amount,
                DiscountType = d.Type
                
            }).ToList();

            return discountVM;

        }
    }
}