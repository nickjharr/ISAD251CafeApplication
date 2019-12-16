using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
            List<Menu> basketObj = JsonConvert.DeserializeObject<List<Menu>>(basket);

            Orders order = new Orders(tableNumber, basketObj);

            order.OrderLines = ConvertToOrderLines(basketObj);

            _context.Orders.Add(order);

            foreach (var ol in order.OrderLines)
            {
                _context.OrderLines.Add(ol);  
            }
            
            _context.SaveChanges();                                        // commit order and orderlines to db
            SetOrderCookie(order);                                         // store order(s) in cookie to retreive late

            HttpContext.Session.Clear();                                   // clear basket once order confirmed to avoid duplicates should 

            return RedirectToAction("OrderConfirmation", order);           //TODO looks gross in URL, only pass the ID and look it up again
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

        public IActionResult OrderConfirmation(Orders order)
        {
            return View(order);
        }

        /// <summary>
        /// Converts a list of Menu items (shopping basket) to List of orderLines
        /// Ensures each item only listed once and allocates appropriate quantity.
        /// </summary>
        private List<OrderLines> ConvertToOrderLines(List<Menu> basketObj)
        {

            List<OrderLines> orderLines = new List<OrderLines>();

            foreach (var item in basketObj)
            {
                OrderLines ol = new OrderLines(item);

                if (orderLines.Contains(ol))
                {
                    int i = 0;
                    while (orderLines[0] != ol)
                    {
                        i++;
                    }

                    orderLines[i].Quantity++;

                }
                else
                {
                    orderLines.Add(ol);
                }
            }

            return orderLines;
        }



        private void SetOrderCookie(Orders order)
        {
            string existingCookieValue = GetOrderCookie();
            List<Orders> orderList = new List<Orders>();

            if (existingCookieValue != null && existingCookieValue != "")
            {
                orderList = JsonConvert.DeserializeObject<List<Orders>>(existingCookieValue);
            }

            orderList.Add(order);

            string cookieValueJson = JsonConvert.SerializeObject(orderList);
            
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMonths(1);
            
            Response.Cookies.Append("orders", cookieValueJson, option);
        }

        private string GetOrderCookie()
        {
            return Request.Cookies["orders"];
        }

    }
}

