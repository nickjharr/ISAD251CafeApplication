using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISAD251CafeApplication.Models;
using Newtonsoft.Json;
using ISAD251CafeApplication.Helpers;

namespace ISAD251CafeApplication.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ItemsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public string GetMenu()
        {
            return JsonConvert.SerializeObject(_context.Menu.ToList());
        }



    }
}