using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Models.ViewModels
{
    public class MenuAndBasketViewModel
    {
        public List<OrderLines> BasketPreview { get; set; }

        public List<Items> Menu { get; set; }
    }
}
