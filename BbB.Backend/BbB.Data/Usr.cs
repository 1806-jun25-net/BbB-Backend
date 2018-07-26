using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class Usr
    {
        public Usr()
        {
            ArchiveOrder = new HashSet<ArchiveOrder>();
            ArchiveUserJoin = new HashSet<ArchiveUserJoin>();
            Driver = new HashSet<Driver>();
            DriverReview = new HashSet<DriverReview>();
            MsgReceiver = new HashSet<Msg>();
            MsgSender = new HashSet<Msg>();
            UserJoin = new HashSet<UserJoin>();
            UserPickup = new HashSet<UserPickup>();
            UserReview = new HashSet<UserReview>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Pass { get; set; }
        public string Company { get; set; }
        public decimal Credit { get; set; }
        public decimal Rating { get; set; }

        public ICollection<ArchiveOrder> ArchiveOrder { get; set; }
        public ICollection<ArchiveUserJoin> ArchiveUserJoin { get; set; }
        public ICollection<Driver> Driver { get; set; }
        public ICollection<DriverReview> DriverReview { get; set; }
        public ICollection<Msg> MsgReceiver { get; set; }
        public ICollection<Msg> MsgSender { get; set; }
        public ICollection<UserJoin> UserJoin { get; set; }
        public ICollection<UserPickup> UserPickup { get; set; }
        public ICollection<UserReview> UserReview { get; set; }
    }
}
