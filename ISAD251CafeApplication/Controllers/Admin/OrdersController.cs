using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using ISAD251CafeApplication.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISAD251CafeApplication.Controllers.Admin
{
    public class OrdersController : Controller
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }
        [Route("[controller]")]
        public IActionResult Index()
        {
            List<Orders> orders = new List<Orders>();

            orders = _context.Orders.OrderBy(x => x.Created).ToList();

            orders = ManageOrderLines.Build(orders, _context);

            return View(orders);
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(int id)
        {
            List<Orders> orders = new List<Orders>();
            Orders orderResult = _context.Orders.Find(id);

            if(orderResult != null)
            {
                orders.Add(_context.Orders.Find(id));
                orders = ManageOrderLines.Build(orders, _context);
            }

            return View(orders);
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult OrderComplete(int id)
        {

            Orders order = _context.Orders.Find(id);
            order.Completed = DateTime.Now;

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}