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
    public class MenuController : Controller
    {
        private readonly StoreContext _context;



        public MenuController(StoreContext context)
        {
            _context = context;
        }


        [Route("[controller]")]
        public IActionResult Index()
        {
            return View(_context.Menu.OrderBy(x => x.ItemCategory));
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult AddBasket(int id)
        {

            List<Menu> basket = new List<Menu>();
            Menu item = _context.Menu.Find(id);
            string value = HttpContext.Session.GetString("basket");

            if (!string.IsNullOrEmpty(value))
            {
                basket = JsonConvert.DeserializeObject<List<Menu>>(value);
            }

            basket.Add(item);

            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction("Index");
        }
    }
}