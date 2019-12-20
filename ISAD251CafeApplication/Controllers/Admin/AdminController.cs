using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISAD251CafeApplication.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly StoreContext _context;

        public AdminController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Orders> orders = new List<Orders>();
            orders = _context.OpenOrders.OrderBy(x => x.Created)
                                        .ToList();

            foreach (Orders o in orders)
            {
                o.OrderLines = _context.OrderLines.Where(x => x.OrderId == o.OrderId)
                                                    .ToList();

                foreach (OrderLines ol in o.OrderLines)
                {
                    //TODO see if this can be taken offline
                    ol.ItemName = _context.Items.Find(ol.ItemId).ItemName;
                }
            }
            return View(orders);
        }

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