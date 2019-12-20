using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models
{
    public class Items
    {
        [Key]
        public int ItemId { get; set; }
        public int ItemCategory { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemStock { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
    }
}
