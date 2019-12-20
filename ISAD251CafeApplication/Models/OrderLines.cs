using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace ISAD251CafeApplication.Models
{
    public class OrderLines
    {

        [Key, Column(Order=0)]
        public int OrderId { get; set; }
        [Key, Column(Order = 1)]
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1; 
        public DateTime? Cancelled { get; set; } = null;
        public DateTime? Completed { get; set; } = null;
        public decimal UnitPrice { get; set; }


        [NotMapped]
        public string ItemName { get; set; }


        public OrderLines(Items item) //int? orderId)
        {
            //OrderId = orderId;
            ItemId = item.ItemId;
            UnitPrice = item.ItemPrice;
        }

        public OrderLines()
        {
            
        }


    }
}
