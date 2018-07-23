using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class ArchiveUserJoin
    {
        public int ArchiveDriveId { get; set; }
        public int UserId { get; set; }

        public ArchiveDrive ArchiveDrive { get; set; }
        public Usr User { get; set; }
    }
}
