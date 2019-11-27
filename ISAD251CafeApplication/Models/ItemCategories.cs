using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models
{
    public class ItemCategories
    {
        [Key]
        public int ItemCategoryId { get; set; }
        public string ItemCategoryDesc { get; set; }
        public bool Active { get; set; }
    }
}
