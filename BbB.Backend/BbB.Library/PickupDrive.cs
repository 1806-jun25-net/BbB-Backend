using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class PickupDrive : Drive
    {
        public static readonly int MAX_PICKUP_SIZE = 12;

        private Dictionary<User,List<OrderItem>> OrdersReal;

        /// <summary>
        /// A new, empty, pickup drive
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="dest"></param>
        /// <param name="time"></param>
        /// <param name="id"></param>
        public PickupDrive(Driver driver, Destination dest, DateTime time, int id = -1):
            base(driver, dest, time, id)
        {
            OrdersReal = null;
        }

        /// <summary>
        /// An enumerable of the users currently on the drive
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<User> Users()
        {
            if(OrdersReal == null)
                OrdersReal = new Dictionary<User, List<OrderItem>>();
            return OrdersReal.Keys;
        }

        /// <summary>
        /// Adds a user to the drive. False if already present or full
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool AddUser(User user)
        {
            if (Users().Contains(user) || Users().Count()>=MAX_PICKUP_SIZE)
                return false;
            OrdersReal.Add(user, new List<OrderItem>());
            return true;
        }

        /// <summary>
        /// Removes a user from the drive. False if not present
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool RemoveUser(User user)
        {
            if (!Users().Contains(user))
                return false;
            OrdersReal.Remove(user);
            return true;
        }

        /// <summary>
        /// Adds an item to the users order. returns false if no user exists
        /// </summary>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(User user, OrderItem item)
        {
            if (!Users().Contains(user))
                return false;
            OrdersReal[user].Add(item);
            return true;
        }

        /// <summary>
        /// returns false if the user or item did not exist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(User user, OrderItem item)
        {
            if (!Users().Contains(user))
                return false;
            return OrdersReal[user].Remove(item);
        }


        /// <summary>
        /// True, this is a pickup
        /// </summary>
        /// <returns></returns>
        public override bool IsPickup()
        {
            return true;
        }
    }
}
