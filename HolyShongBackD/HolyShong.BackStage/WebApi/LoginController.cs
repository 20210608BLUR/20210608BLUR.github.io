using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolyShong.BackStage.Helpers;
using HolyShong.BackStage.ViewModels.Login;

namespace HolyShong.BackStage.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;
        public LoginController()
        {
            _jwtHelper = new JwtHelper();
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginViewModel username)
        {
            if (username.Account == "admin" && username.Password == "admin")
            {                
                return Ok(_jwtHelper.GenerateToken(username.Account + username.Password));
            }
            else
            {
                return NotFound();
            }
        }


    }
}
