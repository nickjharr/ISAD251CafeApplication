using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Helpers;
using ISAD251CafeApplication.Models;
using ISAD251CafeApplication.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            //TODO Investigate. Menu doesn't seem to return view results
            //Brings back all results unless Linq filtered here.
            List<Items> basket = GetBasketAsList();
            List<Items> menuItems = _context.Menu.Where(x => x.Active == true)
                                                    .OrderBy(x => x.ItemCategory)
                                                    .ToList();

            MenuAndBasketViewModel model = new MenuAndBasketViewModel();
            model.Menu = menuItems;
            model.BasketPreview = ConvertToOrderLines(basket);
            ManageOrderLines.PopulateItemNames( model.BasketPreview, _context);

            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
        public IActionResult AddBasket(int itemId)
        {
            //ensure item added to basket exists 
            if (_context.Menu.Find(itemId) != null)
            {
                List<Items> basket = GetBasketAsList();
                if (_context.Menu.Find(itemId) != null)
                {
                    basket.Add(_context.Menu.Find(itemId));
                }
                
                SetBasketInSession(basket);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveItem(int itemId)
        {
            List<Items> items = GetBasketAsList();
            
            foreach (Items item in items.ToList())
            {
                if(item.ItemId == itemId)
                {
                    items.Remove(item);
                }
            }

            SetBasketInSession(items);

            return RedirectToAction("Index");

        }

        public IActionResult Checkout()
        {
            List<OrderLines> orderLines = ConvertToOrderLines(GetBasketAsList());
            ManageOrderLines.PopulateItemNames(orderLines, _context);
            return View(orderLines);
        }

        [Route("[controller]/[action]/{tableNumber}")]
        public IActionResult CompleteOrder(int tableNumber)
        {
            List<Items> basket = GetBasketAsList();
            HttpContext.Session.Clear();

            Orders order = new Orders(tableNumber, basket);
            order.OrderLines = ConvertToOrderLines(basket);
            ManageOrderLines.PopulateItemNames(ref order, _context);

            _context.Orders.Add(order);


            _context.SaveChanges();

            CookieOptions options = new CookieOptions();
            Response.Cookies.Append("orders",
                                   ManageCookies.AppendCookie(ref options, Request.Cookies["orders"], order.OrderId),
                                   options);



            return RedirectToAction("Index", "OrderHistory");           
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
    }
}