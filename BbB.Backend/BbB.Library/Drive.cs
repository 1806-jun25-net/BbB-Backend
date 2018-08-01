using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public abstract class Drive
    {

        public static readonly int MAX_PICKUP_SIZE = 12;

        protected Dictionary<User, List<OrderItem>> OrdersReal;

        protected List<User> UsersReal;

        /// <summary>
        /// internal Id. -1 if untracked by database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Destinaton for the drive
        /// </summary>
        public Destination Dest { get; set; }

        /// <summary>
        /// True if pickup, false if join
        /// </summary>
        public bool IsPickup { get; set; }
        
        /// <summary>
        /// Time the order was placed or will occur
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Number of minutes prior to Time that the drive is finalized
        /// only for active orders
        /// </summary>
        public static readonly int Buffer = 30;

        /// <summary>
        /// Driver exectuing the drive
        /// </summary>
        public Driver Driver { get; set; }

        /// <summary>
        /// A new drive, with given driver
        /// </summary>
        protected Drive(Driver driver, Destination dest, DateTime time, int id = -1)
        {
            OrdersReal = null;
            UsersReal = null;
            Driver = driver;
            Dest = dest;
            Time = time;
            Id = id;
        }

        /// <summary>
        /// true if the order has already happened (is archived)
        /// </summary>
        /// <returns></returns>
        public bool IsPast()
        {
            return (Time < DateTime.Now);
        }

        /// <summary>
        /// current users on this drive
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<User> Users();

        /// <summary>
        /// Adds a user to the drive. false if already in the drive
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract bool AddUser(User user);

        /// <summary>
        /// Removes a user from the drive. false if in the drive
        /// </summary>
        public abstract bool RemoveUser(User user);

    }
}
