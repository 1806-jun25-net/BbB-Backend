using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class Driver
    {
        public Driver()
        {
            ArchiveDrive = new HashSet<ArchiveDrive>();
            Drive = new HashSet<Drive>();
            DriverReview = new HashSet<DriverReview>();
            UserReview = new HashSet<UserReview>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? Seats { get; set; }
        public string MeetLoc { get; set; }

        public Usr User { get; set; }
        public ICollection<ArchiveDrive> ArchiveDrive { get; set; }
        public ICollection<Drive> Drive { get; set; }
        public ICollection<DriverReview> DriverReview { get; set; }
        public ICollection<UserReview> UserReview { get; set; }
    }
}
