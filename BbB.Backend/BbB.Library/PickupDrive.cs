using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class PickupDrive : Drive
    {

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
            IsPickup = true;
        }

        /// <summary>
        /// An enumerable of the users currently on the drive
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<User> Users()
        {
            if (UsersReal == null)
                UsersReal = new List<User>();
            if (OrdersReal == null)
                OrdersReal = new Dictionary<int, List<OrderItem>>();
            return UsersReal;
        }

        /// <summary>
        /// Retrieves the orders for the given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<OrderItem> OrderForUser(User user)
        {
            return OrdersReal[user.Id].ToList();
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
            UsersReal.Add(user);
            OrdersReal.Add(user.Id, new List<OrderItem>());
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
            UsersReal.Remove(user);
            OrdersReal.Remove(user.Id);
            return true;
        }

        /// <summary>
        /// Adds an item to the users order. updates the item qty if already exists. false if no user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(User user, OrderItem item)
        {
            if (!Users().Contains(user))
                return false;
            foreach(OrderItem i in OrdersReal[user.Id])
            {
                if (i.Equals(item))
                {
                    i.Quantity += item.Quantity;
                    return true;
                }
            }
            OrdersReal[user.Id].Add(item);
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
            return OrdersReal[user.Id].Remove(item);
        }
    }
}
