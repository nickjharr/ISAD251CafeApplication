using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models
{
    public class Items
    {


        /// <summary>
        /// Items refers to a model of items in database. 
        /// Intended for admin use, includes items not currently for sale
        /// </summary>

        //TODO Investigate inheritence problem or attempt to implement 1 model for both Items and MenuItems

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
