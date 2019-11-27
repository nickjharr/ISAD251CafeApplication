using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ISAD251CafeApplication.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
           
            string basket = HttpContext.Session.GetString("basket");
            List<Menu> basketObj = JsonConvert.DeserializeObject<List<Menu>>(basket);
            return View(basketObj.OrderBy(x => x.ItemCategory));
        }
    }
}