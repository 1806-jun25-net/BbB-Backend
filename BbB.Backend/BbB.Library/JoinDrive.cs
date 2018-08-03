using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class JoinDrive : Drive
    {
        /// <summary>
        /// New, empty, 'join' drive
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="dest"></param>
        /// <param name="time"></param>
        /// <param name="id"></param>
        public JoinDrive(Driver driver, Destination dest, DateTime time, int id = -1):
            base(driver, dest, time, id)
        {
            IsPickup = false;
        }

        /// <summary>
        /// a copy of users in the order
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<User> Users()
        {
            if (UsersReal == null)
                UsersReal = new List<User>();
            return UsersReal.ToList();
        }

        /// <summary>
        /// adds a user to the drive. false if already present or full
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool AddUser(User user)
        {
            if (Users().Contains(user) || Users().Count() >= Driver.Seats)
                return false;
            UsersReal.Add(user);
            return true;
        }

        /// <summary>
        /// removes a user from the drive. false if not present
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool RemoveUser(User user)
        {
            if (!Users().Contains(user))
                return false;
            UsersReal.Remove(user);
            return true;
        }
    }
}
