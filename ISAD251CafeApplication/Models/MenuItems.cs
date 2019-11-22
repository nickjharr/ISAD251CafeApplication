using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ISAD251CafeApplication.Models
{
    public class MenuItems
    {

        [Key]
        [DisplayName("Item ID")]
        public int ItemId { get; set; }
        [DisplayName("Category")]
        public int ItemCategory { get; set; }
        [DisplayName("Name")]
        public string ItemName { get; set; }
        [DisplayName("Description ")]
        public string ItemDescription { get; set; }
        [DisplayName("Price")]
        public decimal ItemPrice { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
    }
}
