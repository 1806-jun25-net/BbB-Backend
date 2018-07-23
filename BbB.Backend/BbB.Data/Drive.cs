using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class Drive
    {
        public Drive()
        {
            UserJoin = new HashSet<UserJoin>();
            UserPickup = new HashSet<UserPickup>();
        }

        public int Id { get; set; }
        public int DriverId { get; set; }
        public int DestinationId { get; set; }
        public string Dtype { get; set; }
        public DateTime? Dtime { get; set; }

        public Destination Destination { get; set; }
        public Driver Driver { get; set; }
        public ICollection<UserJoin> UserJoin { get; set; }
        public ICollection<UserPickup> UserPickup { get; set; }
    }
}
