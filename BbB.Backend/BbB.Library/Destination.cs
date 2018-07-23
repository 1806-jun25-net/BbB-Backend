using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class Destination
    {
        /// <summary>
        /// Internal Id
        /// -1 if untracked by database
        /// </summary>
        public int Id { get; set;}

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Street Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// All menu items for this location
        /// </summary>
        private List<MenuItem> MenuReal { get; set; }


        /// <summary>
        /// A new location, where name is required
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="address"></param>
        public Destination(string name, int id = -1, string address = "")
        {
            Id = id;
            Name = name;
            Address = address;
            MenuReal = null;
        }

        /// <summary>
        /// All menu items for this location
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuItem> Menu()
        {
            if (MenuReal == null)
                return new List<MenuItem>();
            else
                return MenuReal.ToList();
        }
        /// <summary>
        /// True if the location has a menu of any size
        /// </summary>
        public bool HasMenu()
        {
            return (MenuReal != null);
        }

        /// <summary>
        /// Adds item with given attributes to menu.
        /// If item with name already exists, updates price
        /// returns true if added, false if changed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cost"></param>
        public bool AddItem(string name, decimal cost)
        {
            if (MenuReal == null)
                MenuReal = new List<MenuItem>();
            foreach(MenuItem item in MenuReal)
            {
                if (item.Name == name)
                {
                    item.Cost = cost;
                    return false;
                }
            }
            MenuReal.Add(new MenuItem(name, cost));
            return true;
        }
        
    }
}
