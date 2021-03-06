﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    /// <summary>
    /// Simple compound of string and decimal, with Id for database
    /// </summary>
    public class MenuItem
    {
        public int Id { get; set;  }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public MenuItem(string name, decimal cost, int id = -1)
        {
            Id = id;
            Cost = cost;
            Name = name;
        }

        public bool Equals(MenuItem other)
        {
            return (Cost == other.Cost && Name == other.Name);
        }
    }
}
