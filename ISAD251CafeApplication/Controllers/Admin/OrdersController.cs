using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
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

            orders = BuildOrderlines(orders);

            return View(orders);
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(int id)
        {
            List<Orders> orders = new List<Orders>();
            orders.Add(_context.Orders.Find(id));

            orders = BuildOrderlines(orders);

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

        /// <summary>
        /// A method to iterate through a list of orders and populate the orderlines and item name 
        /// properties.
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private List<Orders> BuildOrderlines(List<Orders> orders)
        {
            //TODO Attempt to optimise this algorithm
            //TODO Handle nulls
            foreach (Orders o in orders)
            {
                o.OrderLines = _context.OrderLines
                    .Where(x => x.OrderId == o.OrderId)
                    .ToList();

                foreach (OrderLines ol in o.OrderLines)
                {
                    ol.ItemName = _context.Items.Find(ol.ItemId).ItemName;
                }
            }

            return orders;
        }

    }
}