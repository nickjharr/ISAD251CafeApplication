using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace ISAD251CafeApplication.Controllers
{
    public class MenuController : Controller
    {
        private readonly StoreContext _context;

        public MenuController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Menu.OrderBy(x=>x.ItemCategory));
        }
    }
}