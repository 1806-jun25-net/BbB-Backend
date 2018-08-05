using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class OrderItem
    {

        public int Id { get; set; }

        /// <summary>
        /// Item if the order is active.
        /// Is an untracked item otherwise.
        /// </summary>
        public MenuItem Item { get; set; }

        /// <summary>
        /// how many of the item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Message attached to item
        /// </summary>
        public string Message { get; set; }

        public OrderItem() { }

        /// <summary>
        /// Item based on existing menu item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <param name="message"></param>
        public OrderItem(MenuItem item, int quantity = 1, string message = "", int id = -1)
        {
            Id = id;
            Item = item;
            Quantity = quantity;
            Message = message;
        }

        /// <summary>
        /// Item based on archived data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cost"></param>
        public OrderItem(string name, decimal cost, int quantity= 1, string message = "") :this(new MenuItem(name, cost), quantity, message)
        { }

        public bool Equals(OrderItem other)
        {
            return (Item.Equals(other.Item) && Message == other.Message);
        }
    }
}
