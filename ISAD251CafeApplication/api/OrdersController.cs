using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Helpers;
using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ISAD251CafeApplication.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Pulls list of open orders from database and return as JSON string.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public string GetOpenOrders()
        {
            List<Orders> openOrders = _context.OpenOrders.Where(x => x.Cancelled == null && x.Completed == null)
                                                            .OrderBy(x => x.Created)
                                                               .ToList();

            foreach(Orders order in openOrders)
            {
                order.Elapsed = (DateTime.Now - order.Created).Minutes;
            }

            ManageOrderLines.Build(openOrders, _context);
            return JsonConvert.SerializeObject(openOrders);
        }


        /// <summary>
        /// Marks order complete in database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public IActionResult OrderComplete([FromForm] int id)
        {           
            Orders order = _context.Orders.Find(id);
            order.Completed = DateTime.Now;
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

    }
}