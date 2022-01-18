using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HolyShong.Repositories;
using HolyShong.Models.HolyShongModel;
using HolyShong.ViewModels;
using System.Data.Entity;

namespace HolyShong.Services
{
    public class AddressService
    {
        private readonly HolyShongRepository _repo;
        private readonly AddressViewModel _addressViewModel;
        public AddressService()
        {
            _repo = new HolyShongRepository();
            _addressViewModel = new AddressViewModel();
        }

        /// <summary>
        /// 取出符合會員Id的所有地址 一個會員可以存很多地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //public List<Address> GetAddresses(int Id)
        //{
        //    var addresses = _repo.GetAll<Address>().Where(x => x.MemberId == Id).ToList();
        //    return addresses;
        //}

        public AddressViewModel GetAddresses(int Id)
        {
            var result = new AddressViewModel()
            {
                AddressDetails = new List<AddressDetail>()
            };
            
            var member = _repo.GetAll<Member>().FirstOrDefault(x => x.MemberId == Id);
            if (member == null)
            {
                return result;
            }
            var addresses = _repo.GetAll<Address>().Where(x => x.MemberId == Id).ToList();

            var addressDetails = addresses.Select(x => new AddressDetail
            { 
                AllAddress = x.AddressDetail,
                IsDefault = x.IsDefault,
                AddressId = x.AddressId

            }).ToList();
            result.AddressDetails = addressDetails;
           
            return result;
    }

       
        /// <summary>
        /// 新增地址
        /// 新的地址設為預設地址，並將舊的預設地址改為非預設地址
        /// </summary>
        /// <param name="request"></param>
        public void CreateAddress(AddressViewModel request)
        {
           
            List<Address> addreses = request.AddressDetails.Select((x, index) => new Address()
            {
                MemberId = request.MemberId,
                //IsDefault = x.IsDefault,
                IsDefault = true,
                CreateTime = DateTime.UtcNow.AddHours(8),
                AddressDetail = x.AllAddress
            }).ToList();

            DbContext context = new HolyShongContext();
            using (var transaction = _repo.Context.Database.BeginTransaction())
                try
                {
                    //將原始預設地址改為非預設地址

                    var memberAddresses = _repo.GetAll<Address>().Where(x => x.MemberId == request.MemberId);
                    foreach(var item in memberAddresses)
                    {
                        item.IsDefault = false;
                        _repo.Update<Address>(item);
                    }
                    _repo.SaveChange();

                    //新增地址 因為一開始設多筆地址 所以新增要FOREACH
                    addreses.ForEach(x =>
                    {
                        _repo.Create<Address>(x);
                       
                    });
                     
                    _repo.SaveChange();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
        }
        /// <summary>
        /// 更新地址
        /// </summary>
        /// <param name="request"></param>
        public void UpdateAddress(AddressViewModel request)
        {
            DbContext context = new HolyShongContext();
            using (var transation = context.Database.BeginTransaction())
                try
                {   
                    // 預設地址
                    var oldDefaultAddress = _repo.GetAll<Address>().FirstOrDefault(x => x.MemberId == request.MemberId && x.IsDefault == true);
                    
                     
                    if (oldDefaultAddress != null)
                    {
                        oldDefaultAddress.IsDefault = false;
                        _repo.Update<Address>(oldDefaultAddress);
                    }
                    var reqAddress = request.AddressDetails.Select(x => new Address()
                    {
                        AddressId = x.AddressId,
                        MemberId = request.MemberId,
                        IsDefault = true,
                        CreateTime = DateTime.UtcNow.AddHours(8),
                        AddressDetail = x.AllAddress
                    }).ToList();
                    reqAddress.ForEach(x =>
                    {
                        _repo.Update<Address>(x);

                    });
                  
                    //更新地址
                    _repo.SaveChange();
                    transation.Commit();
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                }
        }

        /// <summary>
        /// 刪除地址
        /// </summary>
        /// <param name="request"></param>
        public void DeleteAddress(int request)
        {
            DbContext context = new HolyShongContext();
            using (var transaction = context.Database.BeginTransaction())
                try
                {

                    var deleteAddress = FinallyDeleteAddress(request);
                    
                    //var deleteAddress = GetAddresses(request.AddressId).AddressDetails.FirstOrDefault(x => x.IsDefault);
                    //_repo.Update<Address>(deleteAddress);

                    //刪除地址
                    _repo.Delete<Address>(deleteAddress);
                    _repo.SaveChange();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
        }

        // 最後用地址id刪除地址
        public Address FinallyDeleteAddress(int Id)
        {

            var address = _repo.GetAll<Address>().FirstOrDefault(x => x.AddressId == Id);
            

            return address;
        }

    }
}