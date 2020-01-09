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

            //TODO Investigate. OpenOrders doesn't seem to return view results
            //Brings back all results unless Linq filtered here.

            orders = _context.OpenOrders.Where(x=> x.Cancelled == null && x.Completed == null)
                                        .OrderBy(x => x.Created)
                                        .ToList();


            orders = ManageOrderLines.Build(orders, _context);

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