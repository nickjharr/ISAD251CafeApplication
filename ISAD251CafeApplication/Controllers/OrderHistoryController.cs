﻿using System;
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
        [Route("[controller]")]
        public IActionResult Index()
        {

            List<int> orderNumbers = new List<int>();
            string existingCookieValue = GetOrderCookie();

            if (existingCookieValue != null && existingCookieValue != "")
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(GetOrderCookie());
            }

            List<Orders> orders = new List<Orders>();

            foreach (int orderNumber in orderNumbers)
            {
                orders.Add( _context.Orders.Find(orderNumber));
            }

            orders = BuildOrderlines(orders);

            return View(orders);
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(int id)
        {
            List<Orders> orders = new List<Orders>();
            orders.Add( _context.Orders.Find(id));
            orders = BuildOrderlines(orders);


            //TODO propogate null checks - been very lazy 
            //TODO standardise list building either .Add(Stuff) or list = _context.ToList(); - difference could just be lists vs singular
            //TODO standardise sync/async
            
            
            return View(orders);
     
        }

        public async Task<IActionResult> Cancel(int id)
        {
            Orders order = await _context.Orders.FindAsync(id);
            order.Cancelled = DateTime.Now;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", id);
        }

        /// <summary>
        /// A method to iterate through a list of orders and populate the orderlines and item name 
        /// properties.
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private List<Orders> BuildOrderlines(List<Orders> orders)
        {
            foreach (Orders o in orders)
            {
                o.OrderLines = _context.OrderLines
                    .Where(x => x.OrderId == o.OrderId)
                    .ToList();

                foreach (OrderLines ol in o.OrderLines)
                {
                    //TODO see if this can be taken offline
                    ol.ItemName = _context.Items.Find(ol.ItemId).ItemName;
                }
            }

            return orders;
        }

        private string GetOrderCookie()
        {
            return Request.Cookies["orders"];
        }


    }
}