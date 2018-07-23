using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Msg { get; set; }

        public MenuItem Item { get; set; }
        public UserPickup Order { get; set; }
    }
}
