using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace ISAD251CafeApplication.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly StoreContext _context;
        public CheckoutController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string basket = HttpContext.Session.GetString("basket");

            if (basket != null)
            {
                List<Menu> basketObj = JsonConvert.DeserializeObject<List<Menu>>(basket);
                return View(basketObj.OrderBy(x => x.ItemCategory));
            }
            else
            {
                return View();
            }
        }

        [Route("[controller]/[action]/{tableNumber}")]
        public IActionResult CompleteOrder(int tableNumber)
        {

            string basket = HttpContext.Session.GetString("basket");
            HttpContext.Session.Clear(); 

            List<Menu> basketObj = JsonConvert.DeserializeObject<List<Menu>>(basket);

            Orders order = new Orders(tableNumber, basketObj);

            order.OrderLines = ConvertToOrderLines(basketObj);

            _context.Orders.Add(order);
            _context.SaveChanges();                                        // commit order and orderlines to db
            SetOrderCookie(order.OrderId);                                 // store order(s) in cookie to retreive late

            foreach (var ol in order.OrderLines)
            {
                ol.ItemName = _context.Items.Find(ol.ItemId).ItemName;
            }


            return View(order);           //TODO looks gross in URL, only pass the ID and look it up again
        }

        public IActionResult RemoveItem(int id)
        {

            //TODO this can probably be tightened up
            string basket = HttpContext.Session.GetString("basket");
            List<Menu> basketObj = JsonConvert.DeserializeObject<List<Menu>>(basket);

            int i = 0;

            while(i < basketObj.Count && basketObj[i].ItemId != id)
            {
                i++;
            }

            basketObj.RemoveAt(i);
            if(basketObj.Count > 0)
            {
                HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketObj));
            }
            else
            {
                HttpContext.Session.Remove("basket");
            }

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Converts a list of Menu items (shopping basket) to List of orderLines
        /// Ensures each item only listed once and allocates appropriate quantity.
        /// </summary>
        private List<OrderLines> ConvertToOrderLines(List<Menu> basketObj)
        {

            List<OrderLines> orderLines = new List<OrderLines>(); 
          
            //TODO make SRP compliant, factor out checks for duplicates

            foreach (var item in basketObj)
            {
                OrderLines tempOrderLine = new OrderLines(item);
                bool itemAlreadyInList = false;

                if (orderLines.Count == 0)
                {
                    orderLines.Add(tempOrderLine);
                }
                else
                {
                    foreach (var ol in orderLines)
                    {
                        if (ol.ItemId == item.ItemId)
                        {
                            ol.Quantity++;
                            itemAlreadyInList = true;
                        }

                    }
                    if (!itemAlreadyInList)
                    {
                        orderLines.Add(new OrderLines(item));
                    }
                }
            }

            return orderLines;
        }

        private void SetOrderCookie(int orderNumber)
        {
            string existingCookieValue = GetOrderCookie();
            List<int> orderNumbers = new List<int>();

            if (existingCookieValue != null && existingCookieValue != "")
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(existingCookieValue);
            }

            orderNumbers.Add(orderNumber);

            string newCookieValueJson = JsonConvert.SerializeObject(orderNumbers);
            
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMonths(1);
            
            Response.Cookies.Append("orders", newCookieValueJson, option);
        }

        private string GetOrderCookie()
        {
            return Request.Cookies["orders"];
        }

    }
}

