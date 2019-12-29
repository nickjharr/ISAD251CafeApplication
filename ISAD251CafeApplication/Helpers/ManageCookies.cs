using ISAD251CafeApplication.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAD251CafeApplication.Helpers
{
    public static class ManageCookies
    {

        /// <summary>
        /// A method that takes string from cookie and converts to List<Orders>
        /// </summary>
        /// <param name="cookieString"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<Orders> GetOrdersFromCookies(string cookieString, StoreContext context)
        {
            List<int> orderNumbers = new List<int>();
            List<Orders> orders = new List<Orders>();
            Orders orderSingle = new Orders(); ;


            if (cookieString != null && cookieString != "")
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(cookieString);
            }

            foreach (int on in orderNumbers)
            {
                orderSingle = context.Orders.Find(on);

                if (orderSingle != null)
                {
                    orders.Add(orderSingle);
                    orders = ManageOrderLines.Build(orders, context);
                }
            }

            return orders;
        }

        /// <summary>
        /// A Method that deserializes provided cookie JSON, appends the provided order number,reserializes to a string and returns the new string. 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="existingCookie"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string AppendCookie(ref CookieOptions options, string existingCookie, int id)
        {

            List<int> orderNumbers = new List<int>();

            if (existingCookie != null)
            {
                orderNumbers = JsonConvert.DeserializeObject<List<int>>(existingCookie);
            }

            orderNumbers.Add(id);

            string newCookieValueJson = JsonConvert.SerializeObject(orderNumbers);

            ResetExpiry(ref options);

            return newCookieValueJson;
        }

        /// <summary>
        /// A method that resets the expiry date of a CookieOptions type to 1 month
        /// from time executed.
        /// </summary>
        /// <param name="options"></param>
        private static void ResetExpiry(ref CookieOptions options)
        {
            options.Expires = DateTime.Now.AddMonths(1);
        }

    }
}