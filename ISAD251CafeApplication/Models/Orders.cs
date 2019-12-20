using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? Cancelled { get; set; } = null;
        public DateTime? Completed { get; set; } = null;
        public DateTime Created  { get; set; }
        public ICollection<OrderLines> OrderLines { get; set; }

        public Orders(int tableNumber, List<Items> order)
        {
            TableNumber = tableNumber;
            TotalPrice = order.Sum(x => x.ItemPrice);
            Created = DateTime.Now;
            //OrderLines = new HashSet<OrderLines>();
        }

        public Orders()
        {
            
        }
    }
}
