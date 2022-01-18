using System.Collections.Generic;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HolyShong.Models.HolyShongModel;
using HolyShong.Repositories;
using HolyShong.ViewModels;
using HolyShong.Utility;
using HolyShong.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;

namespace HolyShong.Services
{
    public class MemberService
    {
        private readonly HolyShongRepository _repo;
        public MemberService() 
        {
            _repo = new HolyShongRepository();
        }

        public Member UserLogin(MemberLoginViewModel loginVM)
        {
            //使用HtmlEncode將帳密做HTML編碼, 去除有害的字元
            string name = HttpUtility.HtmlEncode(loginVM.Email);
            //string password = HttpUtility.HtmlEncode(loginVM.Password);
            string password = HashService.MD5Hash(HttpUtility.HtmlEncode(loginVM.Password));

            Member user = _repo.GetAll<Member>()
              .SingleOrDefault(x => x.Email.ToUpper() == name.ToUpper() && x.Password == password && x.IsEnable == true)
              ;
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public bool EmailIsUsed(MemberRegisterViewModel registerVM)
        {            
            var emailIsUsed = _repo.GetAll<Member>().FirstOrDefault(x => x.Email == registerVM.Email);
            //email尚未被使用，可以註冊
            if(emailIsUsed == null)
            {
                return false;
            }
            //email已被使用，拒絕註冊
            else
            {
                return true;
            }

        }

        public bool CreateAccount(MemberRegisterViewModel registerVM, ref Member account,ref Address ad) 
        {
            if (registerVM == null)
            {
                return false;
            }

            //View Model -> Data Model, 並以HtmlEncode做安全性編碼
            account = new Member
            {
                FirstName = HttpUtility.HtmlEncode(registerVM.FirstName),
                LastName = HttpUtility.HtmlEncode(registerVM.LastName),
                Password = HashService.MD5Hash(HttpUtility.HtmlEncode(registerVM.Password)),
                CreateTime = DateTime.UtcNow.AddHours(8),
                Email = HttpUtility.HtmlEncode(registerVM.Email),
                Cellphone = HttpUtility.HtmlEncode(registerVM.Cellphone),
                IsEnable = false,
                IsDelete = false,
                ActivetionCode = Guid.NewGuid(),
                EffectiveTime = DateTime.UtcNow.AddHours(8).AddMinutes(15)
            };
            ad = new Address
            {
                IsDefault = true,
                CreateTime = DateTime.UtcNow.AddHours(8),
                AddressDetail = HttpUtility.HtmlEncode(registerVM.AddressDetails)
            };


            bool status = false;

            _repo.Create<Member>(account);
            _repo.Create<Address>(ad);
            _repo.SaveChange();
                   
            status = true;

            return status;
        }

        public void SendEmail(HttpRequestBase request, UrlHelper urlHelper , string email, Guid? activetionCode )
        {
            var effectiveTime = DateTime.UtcNow.AddHours(8).AddMinutes(15);
            var route = new RouteValueDictionary { { "ActivetionCode", activetionCode},{"Email", email },{ "EffectiveTime", effectiveTime } };   
            var link = urlHelper.Action("ConfirmE","Member", route, request.Url.Scheme, request.Url.Host);

            var fromMail = new MailAddress("holyshong2022@gmail.com"); // set your email    
            var fromEmailpassword = "Hs20211014"; // Set your password     
             
            var toEmail = new MailAddress(email); //撈DB建立的資料

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "給你送!註冊完成!!";
            Message.SubjectEncoding = Encoding.UTF8;
            Message.Body =
            $"<br/> 你成功註冊了一個帳號!請點擊下方連結來登入會員!"+
            $"<br/><a href=\" {link} \">點擊此連結</a>";
            
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = true;
            try
            {
                smtp.Send(Message);
                Message.Dispose(); //釋放資源
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        
        public bool SetIsEnable(Member memberM)
        {
            var member = (memberM.ActivetionCode).ToString();
            
            Member user = _repo.GetAll<Member>().SingleOrDefault(x => x.ActivetionCode.ToString() == member);

            user.IsEnable = true;
            _repo.Update<Member>(user);
            _repo.SaveChange();


            return true;
        }

        public bool ActiveCodeEmailConfirm(string activetionCode,string email) 
        {
            var result =
            _repo.GetAll<Member>().Any(x => x.Email == email && x.ActivetionCode.ToString() == activetionCode);

            
            return result;
        }
                

    }
}