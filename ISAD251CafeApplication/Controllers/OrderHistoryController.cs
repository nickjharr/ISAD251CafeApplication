using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using ISAD251CafeApplication.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ISAD251CafeApplication.Models.ViewModels;

namespace ISAD251CafeApplication.Controllers
{
    public class OrderHistoryController : Controller
    {
        private readonly StoreContext _context;

        public OrderHistoryController(StoreContext context)
        {
            _context = context;
        }

        [ActionName("Index")]
        [Route("[controller]")]
        public IActionResult Index()
        {

            string test = Request.Cookies["orders"];
            List<Orders> orders = ManageCookies.GetOrdersFromCookies(test, _context);
            orders = orders.OrderByDescending(x => x.Created).ToList();

            orders = ManageOrderLines.Build(orders, _context);

            return View(orders);
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(int id)
        {
            List<Orders> orders = new List<Orders>();
            Orders orderSingle = new Orders();

            orderSingle = _context.Orders.Find(id);

            if (orderSingle != null)
            {
                orders.Add(orderSingle);
                orders = ManageOrderLines.Build(orders, _context);
            }

            return View(orders);
     
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public IActionResult Cancel(int orderId)
        {
            Orders order = _context.Orders.Find(orderId);
            order.Cancelled = DateTime.Now;

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}