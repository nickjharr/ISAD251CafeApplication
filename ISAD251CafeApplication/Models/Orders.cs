using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Cancelled { get; set; }
        public bool Completed { get; set; }
        public DateTime Created  { get; set; }


    }
}
