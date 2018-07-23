using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class ArchiveDrive
    {
        public ArchiveDrive()
        {
            ArchiveOrder = new HashSet<ArchiveOrder>();
            ArchiveUserJoin = new HashSet<ArchiveUserJoin>();
            DriverReview = new HashSet<DriverReview>();
            UserReview = new HashSet<UserReview>();
        }

        public int Id { get; set; }
        public int DriverId { get; set; }
        public int DestinationId { get; set; }
        public string Dtype { get; set; }
        public DateTime? Dtime { get; set; }

        public Destination Destination { get; set; }
        public Driver Driver { get; set; }
        public ICollection<ArchiveOrder> ArchiveOrder { get; set; }
        public ICollection<ArchiveUserJoin> ArchiveUserJoin { get; set; }
        public ICollection<DriverReview> DriverReview { get; set; }
        public ICollection<UserReview> UserReview { get; set; }
    }
}
