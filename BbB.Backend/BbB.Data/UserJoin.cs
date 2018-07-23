using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class UserJoin
    {
        public int DriveId { get; set; }
        public int UserId { get; set; }

        public Drive Drive { get; set; }
        public Usr User { get; set; }
    }
}
