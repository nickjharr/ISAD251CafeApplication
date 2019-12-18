using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {

            List<int> orderNumbers = new List<int>();
            string existingCookieValue = GetOrderCookie();

            if (existingCookieValue != null && existingCookieValue != "")
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(GetOrderCookie());
            }

            List<Orders> fullOrders = new List<Orders>();

            foreach (int orderNumber in orderNumbers)
            {
                fullOrders.Add(await _context.Orders.FindAsync(orderNumber));
            }

            return View(fullOrders);
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

        public async Task<IActionResult> Cancel(int id)
        {
            Orders order = await _context.Orders.FindAsync(id);
            order.Cancelled = DateTime.Now;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", id);
        }


        public IActionResult Details(int id)
        {
            //TODO tidy up construction of model
            Orders order = _context.Orders.Find(id);            
            order.OrderLines = _context.OrderLines.Where(x => x.OrderId == order.OrderId).ToList();
           
            foreach(OrderLines ol in order.OrderLines)
            {
                ol.ItemName = _context.Items.Find(ol.ItemId).ItemName;
            }

            return View(order);
        }

        private string GetOrderCookie()
        {
            return Request.Cookies["orders"];
        }


    }
}