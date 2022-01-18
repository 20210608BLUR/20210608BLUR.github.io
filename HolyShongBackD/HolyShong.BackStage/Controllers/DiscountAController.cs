using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HolyShong.BackStage.Controllers
{
    public class DiscountAController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
