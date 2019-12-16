using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISAD251CafeApplication.Controllers
{
    public class OrderHistoryController : Controller
    {
        private readonly StoreContext _context;

        public OrderHistoryController(StoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            List<Orders> ordersCookie = new List<Orders>();
            string test = GetOrderCookie();
            if (GetOrderCookie() != null)
            {
                ordersCookie = JsonConvert.DeserializeObject<List<Orders>>(GetOrderCookie());
            }

            return View(ordersCookie);
        }

        [Route("[controller]/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            Orders result = await _context.Orders.FindAsync(id);

            if (result != null)
            {
                List<Orders> results = new List<Orders>();
                results.Add(result);
                return View(results);
            }
            else
            {
                return View(null);
            }
     
            
        }


        private string GetOrderCookie()
        {
            return Request.Cookies["orders"];
        }


    }
}