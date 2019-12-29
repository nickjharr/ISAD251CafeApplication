using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISAD251CafeApplication.Models;


namespace ISAD251CafeApplication.Helpers
{
    public static class ManageOrderLines
    {
        /// <summary>
        /// A method to iterate through a list of orders and populate the orderlines and item name properties.
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<Orders> Build(List<Orders> orders, StoreContext context)
        {
            //TODO Attempt to optimise this algorithm
            if (orders != null)
            {
                foreach (Orders o in orders)
                {
                    o.OrderLines = context.OrderLines
                        .Where(x => x.OrderId == o.OrderId)
                        .ToList();


                    foreach (OrderLines ol in o.OrderLines)
                    {
                        ol.ItemName = context.Items.Find(ol.ItemId).ItemName;
                    }
                }
            }

            return orders;
        }

        /// <summary>
        /// A method to populate item names in order models.
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="context"></param>
        public static void PopulateItemNames(ref Orders orders, StoreContext context)
        {
            foreach (var ol in orders.OrderLines)
            {
                ol.ItemName = context.Items.Find(ol.ItemId).ItemName;
            }
        }

        public static void PopulateItemNames(List<OrderLines> orderLines, StoreContext context)
        {
            foreach (var ol in orderLines)
            {
                ol.ItemName = context.Items.Find(ol.ItemId).ItemName;
            }
        }


    }
}
