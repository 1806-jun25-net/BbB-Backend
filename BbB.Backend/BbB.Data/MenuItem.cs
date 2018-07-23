using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class MenuItem
    {
        public MenuItem()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int DestinationId { get; set; }
        public string ItemName { get; set; }
        public decimal? Cost { get; set; }

        public Destination Destination { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}
