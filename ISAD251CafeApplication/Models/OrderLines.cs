using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models
{
    public class OrderLines
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime Cancelled { get; set; }
        public DateTime Completed { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
