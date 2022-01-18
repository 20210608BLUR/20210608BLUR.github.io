using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HolyShong.Services;
using HolyShong.ViewModels;

namespace HolyShong.WebAPI
{    
    public class AddressApiController : ApiController
    {
        private readonly AddressService _addressService;
        public AddressApiController()
        {
            _addressService = new AddressService();
        }

        
        //[Authorize]
        public IHttpActionResult GetAllAddresses()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                var addresses = _addressService.GetAddresses(int.Parse(User.Identity.Name));
                return Ok(addresses);
            }
            return Ok();
        }


        [HttpPost]
        public IHttpActionResult CreateAddress(AddressViewModel request)
        {
            if(string.IsNullOrEmpty(User.Identity.Name))
            {
                return Ok();
            }else
            {
                request.MemberId = int.Parse(User.Identity.Name);
                _addressService.CreateAddress(request);
                return Ok();
            }

            
        }


        [HttpPost]
        public IHttpActionResult UpdateAddress(AddressViewModel request)
        {
            request.MemberId = int.Parse(User.Identity.Name);
            _addressService.UpdateAddress(request);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteAddress(int request)
        {
            _addressService.DeleteAddress(request);
            return Ok();
        }
    }
}
