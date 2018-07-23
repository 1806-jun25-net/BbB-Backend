using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class DriverReview
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public int ArchiveDriveId { get; set; }

        public ArchiveDrive ArchiveDrive { get; set; }
        public Driver Driver { get; set; }
        public Usr User { get; set; }
    }
}
