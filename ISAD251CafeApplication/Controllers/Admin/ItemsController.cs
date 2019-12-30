using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISAD251CafeApplication.Controllers.Admin
{
    public class ItemsController : Controller
    {
        private readonly StoreContext _context;

        public ItemsController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Items.OrderBy(x => x.ItemCategory));
        }

        [Route("[controller]/{term}")]
        public IActionResult Index(string term)
        {
            List<Items> results = new List<Items>();

            try
            {
                int id = Convert.ToInt32(term);
                List<Items> idResults = _context.Items.Where(x => x.ItemId == id).ToList();
                results = idResults;
            }
            catch (FormatException)
            {
                //if entered value not an integer, search for the string in item name

                List<Items> nameResults =_context.Items.Where(x => x.ItemName.Contains(term)).ToList();
                results = nameResults;

                List<Items> descResults = _context.Items.Where(x => x.ItemDescription.Contains(term)).ToList();

                foreach (Items item in descResults)
                {
                    results.Add(item);
                }

            }

            return View(results.Distinct());
        }

        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public IActionResult Create([Bind("ItemId,ItemCategory,ItemName,ItemDescription,ItemPrice,ItemStock,Active")] Items newItem)
        {
            
            _context.Entry(newItem).State = EntityState.Added;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) 
        {
            return View(_context.Items.Find(id));
        }

        [HttpPost]
        public ActionResult Edit([Bind("ItemId,ItemCategory,ItemName,ItemDescription,ItemPrice,ItemStock,Active, Created")] Items updatedItem)
        {
           if (ModelState.IsValid)
            {

                _context.Entry(updatedItem).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// A method to toggle the status of an item/product. If current on sale will be removed, 
        /// and vice versa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ToggleMenuStatus(int id)
        {
            Items item = _context.Items.Find(id);
            item.Active = !item.Active;

            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}