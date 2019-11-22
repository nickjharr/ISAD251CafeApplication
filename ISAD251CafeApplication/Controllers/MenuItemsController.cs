using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISAD251CafeApplication.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly StoreContext _context;



        public MenuItemsController(StoreContext context)
        {
            _context = context;
        }


        [Route("[controller]")]
        public IActionResult Index()
        {
            return View(_context.MenuItems.OrderBy(x => x.ItemCategory));
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult AddBasket(int id)
        {
            //
            var test = HttpContext.Session.GetString("basket");
            //

            List<MenuItems> basket = new List<MenuItems>();
            MenuItems item = _context.MenuItems.Find(id);
            string value = HttpContext.Session.GetString("basket");         
           
            if (!string.IsNullOrEmpty(value))
            {
                basket = JsonConvert.DeserializeObject<List<MenuItems>>(value);
            }

            basket.Add(item);

            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));

            //
            test = HttpContext.Session.GetString("basket");
            //

            return RedirectToAction("Index");
        }


    }
}