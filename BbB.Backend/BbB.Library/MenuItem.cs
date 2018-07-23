using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    /// <summary>
    /// Simple compound of string and decimal
    /// </summary>
    public class MenuItem
    {
        public string Name {get;}
        public decimal Cost { get; set; }
        public MenuItem(string name, decimal cost)
        {
            Cost = cost;
            Name = name;
        }
    }
}
