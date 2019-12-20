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
        
            return View(_context.Items.ToList().OrderBy(x=>x.ItemCategory));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
        public ActionResult Edit([Bind("ItemId,ItemCategory,ItemName,ItemDescription,ItemPrice,ItemStock,Active,Created")] Items updatedItem)
        {
            //TODO propogate this method of updating stuff 
            if (ModelState.IsValid)
            {
                _context.Entry(updatedItem).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(_context.Items.Find(id));
        }
        
        [Route("[controller]/[action]/{term}")]
        public IActionResult ItemSearch(string term)
            {
            return View(_context.Items.Where(x => x.ItemName.Contains(term)));
                
                
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