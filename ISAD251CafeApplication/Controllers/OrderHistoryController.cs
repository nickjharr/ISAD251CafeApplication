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

    //TODO propogate null checks - been very lazy 
    //TODO standardise list building either .Add(Stuff) or list = _context.ToList(); - difference could just be lists vs singular
    //TODO standardise sync/async


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
            List<Orders> orders = GetOrdersFromCookies();
            orders = orders.OrderBy(x => x.Created).ToList();
            return View(orders);
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(int id)
        {
            List<Orders> orders = new List<Orders>();
            orders.Add( _context.Orders.Find(id));
            orders = BuildOrderlines(orders);

            return View(orders);
     
        }

        public IActionResult Cancel(int id)
        {
            Orders order = _context.Orders.Find(id);
            order.Cancelled = DateTime.Now;

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
            if (orders != null)
            {
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
            }

            return orders;
        }

        /// <summary>
        /// A method that reads the sites cookies and retreives the customers order history. 
        /// Converts from string to List<Items>
        /// </summary>
        /// <returns></returns>
        private List<Orders> GetOrdersFromCookies()
        {
            string cookieString = Request.Cookies["orders"];
            List<int> orderNumbers = new List<int>();
            List<Orders> orders = new List<Orders>();
            if (cookieString != null && cookieString != "")
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(cookieString);
            }

            foreach(int on in orderNumbers)
            {
                orders.Add(_context.Orders.Find(on));
            }

            orders = BuildOrderlines(orders);

            return orders;
        }

    }
}