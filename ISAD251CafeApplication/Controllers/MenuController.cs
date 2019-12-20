using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISAD251CafeApplication.Controllers
{
    public class MenuController : Controller
    {
        private readonly StoreContext _context;

        public MenuController(StoreContext context)
        {
            _context = context;
        }

        [Route("[controller]")]
        public IActionResult Index()
        {
            return View(_context.Menu.OrderBy(x => x.ItemCategory));
        }

        //TODO HttpPost?
        [Route("[controller]/[action]/{id}")]
        public IActionResult AddBasket(int id)
        {
            List<Items> basket = GetBasketAsList();

            UpdateQuantities(basket, id);
            SetBasketInSession(basket);
           
            return RedirectToAction("Index");
        }              

        public IActionResult Checkout()
        {
            return View(GetBasketAsList());
        }

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



        private List<Items> GetBasketAsList()
        {
            List<Items> basket = new List<Items>();

            string sessionBasketString = HttpContext.Session.GetString("basket");

            if (!string.IsNullOrEmpty(sessionBasketString))
            {
                basket = JsonConvert.DeserializeObject<List<Items>>(sessionBasketString);
            }

            return basket;
        }

        private void SetBasketInSession(List<Items> basket)
        {
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));
        }

        /// <summary>
        /// Removes duplicate entries in list and updates remaining entry's
        /// quantity property as required. 
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="id"></param>
        
        
        private void UpdateQuantities(List<Items> basket, int id)
        {
            int i = 0;
            bool duplicateFound = false;
            Items tempItem;

            while (duplicateFound == false && i < basket.Count)
            {
                if (basket[i].ItemId == id)
                {
                    tempItem = basket[i];
                    basket.Add(tempItem);
                    duplicateFound = true;
                }

                i++;
            }

            if (duplicateFound != true)
            {
                Items item = _context.Menu.Find(id);
                basket.Add(item);
            }
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





    }
}