using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Controllers
{
    public class StoreController : Controller
    {
        //餐廳Table
        public IActionResult Index()
        {
            return View();
        }

        //餐廳create
        public IActionResult AddStore()
        {
            return View();
        }

        //餐廳update
        public IActionResult ModifyStore() 
        {
            return View();
        }

        //餐點列表
        public IActionResult ProductList()
        {
            return View();
        }

        public IActionResult AddAllStore()
        {
            return View();
        }

    }
}
