using HolyShong.Models;
using HolyShong.ViewModels;
using HolyShong.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HolyShong.Repositories;
using HolyShong.Models.HolyShongModel;
using HolyShong.Utility;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HolyShong.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberService _memberService;
        private readonly MemberProfileService _memberProfileService;
        private readonly OrderService _orderService;
        private readonly FavoriteService _favoriteService;
        private readonly CheckVipDate _checkVipDate;
        private readonly StoreRegisterService _storeRegisterService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly DeliverRegisterService _deliverRegisterService;


        public MemberController()
        {
            _memberService = new MemberService();
            _memberProfileService = new MemberProfileService();
            _orderService = new OrderService();
            _favoriteService = new FavoriteService();
            _checkVipDate = new CheckVipDate();
            _cartService = new CartService();
            _productService = new ProductService();
            _storeRegisterService = new StoreRegisterService();
            _cartService = new CartService();
            _deliverRegisterService = new DeliverRegisterService();
        }

        /// <summary>
        /// VUP說明頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Eatpass()
        {
            int memberId = 0;
            var isLogin = int.TryParse(User.Identity.Name, out memberId);
            if (isLogin == false)
            {
                return RedirectToAction("Login", "Member");
            }
            var model = _memberProfileService.GetMemberProfileViewModel(memberId);
            return View(model);
        }
   
        /// <summary>
        /// VIP頁面說明頁面 修改資料
        /// </summary>
        /// <param name="memberProfileViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Eatpass(UserProfileViewModel memberProfileViewModel)
        {
            _memberProfileService.EditMemberProfile(memberProfileViewModel);
            return View(memberProfileViewModel);
        }
        

        public ActionResult Checkout()
            {
                return View();
            }
        
        /// <summary>
        /// 會員個人資料頁面
        /// 讀取資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {

            var model = _memberProfileService.GetMemberProfileViewModel(int.Parse(User.Identity.Name));

            return View(model);

        }

        /// <summary>
        /// 會員個人資料頁面
        /// 修改資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfileViewModel memberProfileViewModel)
        {
            memberProfileViewModel.IsUpdate = false;
            //string 
            if (ModelState.IsValid)
            {
                bool result = _memberProfileService.EditMemberProfile(memberProfileViewModel);
                if (result)
                {
                    memberProfileViewModel.IsUpdate = true;
                }
            }
            return View(memberProfileViewModel);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// 外送員註冊頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult DeliverRegister()
        {
            var memberId = int.Parse(User.Identity.Name);
            var deliverRegisterViewModel = _deliverRegisterService.CheckStatus(memberId);
            if (deliverRegisterViewModel.IsDeliver)
            {
                return RedirectToAction("Index", "Deliver");
            }
            return View(deliverRegisterViewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeliverRegister(DeliverRegisterViewModel deliverRegisterViewModel)
        {
            var saveResult = _deliverRegisterService.CreateDeliver(deliverRegisterViewModel);
            if (saveResult)
            {
                var memberId = deliverRegisterViewModel.MemberId;
                var newDeliverRegisterViewModel = _deliverRegisterService.CheckStatus(memberId);
                if (newDeliverRegisterViewModel.IsDeliver)
                {
                    return RedirectToAction("Index", "Deliver");
                }
                return View(newDeliverRegisterViewModel);
            }
            else
            {

                return Content("系統錯誤，請聯絡管理員");
            }

            //var newdeliverRegisterViewModel
        }

        /// <summary>
        /// 最愛店家頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult FavoriteStore()
        {
            var memberId = int.Parse(User.Identity.Name);

            var result = _favoriteService.GetFavorite(memberId);

            if (result.favoriteStores == null)
            {
                return RedirectToAction("NoSearch", "Home");
            }

            return View(result);

        }

        /// <summary>
        /// 刪除喜愛店家 
        /// </summary>
        /// <returns></returns>
        public ActionResult FavoriteStoreDelete(string store)
        {
            var storeId = int.Parse(string.Join("", store.Skip(6)));
            //int restaurantId = (int)System.Web.HttpContext.Current.Session["storeId"];
            var memberId = 0;
            var isLogin = int.TryParse(User.Identity.Name, out memberId);
            _productService.FavoriteDelete(storeId, memberId);
            return RedirectToAction("FavoriteStore", "Member");
        }
        public ActionResult OrderList()
        {
            var memberId = int.Parse(User.Identity.Name);
            if (memberId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = _orderService.GetOrderByMemberId(memberId);
            return View(result);
        }

        /// <summary>
        /// 餐廳註冊頁面 讀取資料
        /// 確認申請人狀態
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult RestaurantRegister()
        {
            var memberId = int.Parse(User.Identity.Name);

            var storeRegisterViewModel = _storeRegisterService.MemberStatus(memberId);

            return View(storeRegisterViewModel);
        }

        /// <summary>
        /// 餐廳註冊
        /// </summary>
        /// <param name="storeRegisterViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult RestaurantRegister(StoreRegisterViewModel storeRegisterViewModel)
        {
            var result = _storeRegisterService.CreateStore(storeRegisterViewModel);
            if (result)
            {
                var newStoreRegisterViewModel = _storeRegisterService.MemberStatus(storeRegisterViewModel.MemberId);
                return View(newStoreRegisterViewModel);
            }
            else
            {
                return Content("系統錯誤，請聯絡管理員");
            }
        }

        public ActionResult Invite()
        {
            return View();
        }
        /// <summary>
        /// 會員註冊頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 會員註冊資料
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MemberRegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            
            registerVM.MailIsUsed = false;
            //判斷email是否已註冊
            bool emailIsUsed = _memberService.EmailIsUsed(registerVM);
            if (emailIsUsed)
            {
                registerVM.MailIsUsed = true;
                return View(registerVM);
            }

            Member account = new Member();
            Address ad = new Address();

            bool status = _memberService.CreateAccount(registerVM, ref account, ref ad);

            if (status)
            {
                //呼叫寄信(寫在這)
                _memberService.SendEmail(Request, Url, account.Email, account.ActivetionCode);
                
                //return Content("新增帳號成功,請收信進行驗證完成註冊!");
                return RedirectToAction("ShowModal", "Member");
                
            }
            else
            {
                //return Content("新增帳號失敗,請重新註冊!");
                return RedirectToAction("FailModal", "Member");
            }
            
        }

        public ActionResult ShowModal() 
        {
            return View();
        }

        public ActionResult FailModal() 
        {
            return View();
        }

        /// <summary>
        /// 確認信件
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ConfirmE(Member halfMember, string activetionCode, DateTime effectiveTime)
        {
            
            var dbEmail = halfMember.Email;
            var nowTime = DateTime.UtcNow.AddHours(8);

            //驗證時間是否通過
            if (nowTime > effectiveTime) {
                return RedirectToAction("FailModal", "Member");
            }
            //用信箱&驗證碼找DB裡相同的資料
            var isMember = _memberService.ActiveCodeEmailConfirm(activetionCode, dbEmail);
            //驗證驗證碼
            if (isMember == false)
            {
                return RedirectToAction("FailModal", "Member");
            }

            //驗證信箱

            _memberService.SetIsEnable(halfMember);
            return RedirectToAction("Login", "Member");

        }



        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MemberLoginViewModel loginVM)
        {
            //一.未通過驗證
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            //二.通過Model驗證後, 使用HtmlEncode將帳密做HTML編碼, 去除有害的字元
            string name = HttpUtility.HtmlEncode(loginVM.Email);
            //string password = HttpUtility.HtmlEncode(loginVM.Password);
            string password = HashService.MD5Hash(HttpUtility.HtmlEncode(loginVM.Password));

            //三.EF比對資料庫帳密
            //以Name及Password查詢比對Account資料表記錄

            Member user = _memberService.UserLogin(loginVM);
            //Account user = _ctx.Accounts.Where(x => x.Name.ToUpper() == name.ToUpper() && x.Password == password).FirstOrDefault();

            //Account user2 = _ctx.Accounts.SingleOrDefault(x => x.Name.ToUpper() == name.ToUpper() && x.Password == password);
            //找不到則彈回Login頁
            if (user == null)
            {
                ModelState.AddModelError("Password", "無效的帳號或密碼!");

                return View(loginVM);
            }

            //四.FormsAuthentication Class -- https://docs.microsoft.com/zh-tw/dotnet/api/system.web.security.formsauthentication?view=netframework-4.8

            //FormsAuthenticationTicket Class
            //https://docs.microsoft.com/zh-tw/dotnet/api/system.web.security.formsauthenticationticket?view=netframework-4.8


            //1.建立FormsAuthenticationTicket
            var ticket = new FormsAuthenticationTicket(
            version: 1,
            name: user.MemberId.ToString(), //可以放使用者Id
            issueDate: DateTime.UtcNow,//現在UTC時間
            expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
            isPersistent: loginVM.Remember,// 是否要記住我 true or false
            userData: user.LastName.ToString(), //可以放使用者角色名稱
            cookiePath: FormsAuthentication.FormsCookiePath);

            //2.加密Ticket
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            //3.Create the cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);

            //4.取得original URL.
            //如果沒有上一頁,到預設首頁
            string url;
            if(loginVM.Url == null)
            {
                url = FormsAuthentication.GetRedirectUrl(name, true);
            }
            else
            {
                url = loginVM.Url;
            }

            #region cookie導入memberID，用來限制使用者讀取他人資料

            ////設定cookie值為MemberId
            //HttpCookie idCookie = new HttpCookie("memberIdcookie");
            //idCookie.Value = name;
            ////指定 Cookie 的有效日期，過了有效日期 Cookie 就不會再儲存；不指定這個參數，有效日期就是使用者退出瀏覽器時
            ////指定有效時間30分鐘(同上)
            //idCookie.Expires = DateTime.UtcNow.AddMinutes(30);
            ////指定可以存取該 Cookie 的路徑；不指定這個參數，預設該 Cookie 的網頁所在的路徑
            //idCookie.Path = "/";
            ////指定可以存取該 Cookie 的網域；不指定這個參數，預設該 Cookie 的網頁所在的網域
            //idCookie.Domain = "";
            ////指定 Cookie 只可以傳送給 HTTPS 伺服器
            //idCookie.Secure = false;
            //Response.Cookies.Add(idCookie);

            #endregion

            _checkVipDate.CheckVip(user.MemberId);

            if (user.MemberId != 0)
            {
                //登入後seesion內容成購物車
                if (Session["Cart"] != null)
                {
                    //判斷店家是否相同
                    var cartListVM = (List<StoreProduct>)Session["Cart"];
                    var isStoreMatch = _cartService.MatchCartStore(cartListVM, user.MemberId);
                    //店家不同
                    if (!isStoreMatch)
                    {
                        //刪除舊的購物車，加入新的購物車
                        var cart = _cartService.ChangeCart(cartListVM, user.MemberId);
                        Session.Remove("Cart");
                    }
                }
            }

            //5.導向original URL
            return Redirect(url);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            //Response.Cookies.Remove(cookie);
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}