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
            return View(GetBasketAsList());
        }

        [Route("[controller]/[action]/{tableNumber}")]
        public IActionResult CompleteOrder(int tableNumber)
        {
            List<Items> basket = GetBasketAsList();
            HttpContext.Session.Clear();

            Orders order = new Orders(tableNumber, basket);
            order.OrderLines = ConvertToOrderLines(basket);
            PopulateItemNames(ref order);

            _context.Orders.Add(order);

            _context.SaveChanges();                                        // commit order and orderlines to db
            AppendOrderCookie(order.OrderId);                              // store order(s) in cookie to retreive late

            return View(order);           //TODO looks gross in URL, only pass the ID and look it up again
        }

        public IActionResult RemoveItem(int id)
        {

            List<Items> basket = GetBasketAsList();

            int i = 0;

            while(i < basket.Count && basket[i].ItemId != id)
            {
                i++;
            }

            basket.RemoveAt(i);

            if(basket.Count > 0)
            {
                SetBasketInSession(basket);
            }
            else
            {
                HttpContext.Session.Remove("basket");
            }

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Converts a List<Items> (shopping basket) to List of orderLines
        /// Ensures each item only listed once and allocates appropriate quantity.
        /// </summary>
        private List<OrderLines> ConvertToOrderLines(List<Items> basketObj)
        {

            List<OrderLines> orderLines = new List<OrderLines>(); 
          

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

        private List<Items> GetBasketAsList()
        {
            string basketString = HttpContext.Session.GetString("basket");
            List<Items> basketList = new List<Items>();
            
            if(basketString != null && basketString != "")
            {
                basketList = JsonConvert.DeserializeObject<List<Items>>(basketString);
                basketList.OrderBy(x => x.ItemCategory);
            }

            return basketList;
        }

        private void AppendOrderCookie(int id)
        {
            string existingCookie = Request.Cookies["orders"];

            List<int> orderNumbers = new List<int>();

            if (existingCookie != null)
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(existingCookie);
            }

            orderNumbers.Add(id);

            string newCookieValueJson = JsonConvert.SerializeObject(orderNumbers);
            
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMonths(1);
            
            Response.Cookies.Append("orders", newCookieValueJson, option);
        }

        private void PopulateItemNames(ref Orders order)
        {
            foreach (var ol in order.OrderLines)
            {
                ol.ItemName = _context.Items.Find(ol.ItemId).ItemName;
            }
        }

        private void SetBasketInSession(List<Items> basket)
        {
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));
        }

    }
}

