using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISAD251CafeApplication.Models
{
    public class Menu 
    {

        [Key]
        public int ItemId { get; set; }
        public int ItemCategory { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
    }
}
