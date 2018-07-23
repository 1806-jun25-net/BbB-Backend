using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class UserPickup
    {
        public UserPickup()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int DriveId { get; set; }

        public Drive Drive { get; set; }
        public Usr User { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}
