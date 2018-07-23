using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class OrderItem
    {
        /// <summary>
        /// Item if the order is active.
        /// Is an untracked item otherwise.
        /// </summary>
        private MenuItem Item { get; set; }

        /// <summary>
        /// how many of the item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Message attached to item
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Item based on existing menu item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <param name="message"></param>
        public OrderItem(MenuItem item, int quantity=1, string message = "")
        {
            Item = item;
            Quantity = quantity;
            Message = message;
        }

        /// <summary>
        /// Item based on archived data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cost"></param>
        public OrderItem(string name, decimal cost):this(new MenuItem(name, cost))
        { }
    }
}
