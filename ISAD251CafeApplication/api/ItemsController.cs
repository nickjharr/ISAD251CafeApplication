using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISAD251CafeApplication.Models;

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

        //API Get
        [HttpGet]
        public IEnumerable<Items> GetMenu()
        {
            return _context.Items;
        }
    }
}