using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace ISAD251CafeApplication.Controllers
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
            return View(_context.Menu);
        }
        
    }
}