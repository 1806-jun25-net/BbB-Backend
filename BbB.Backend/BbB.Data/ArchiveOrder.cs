using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class ArchiveOrder
    {
        public ArchiveOrder()
        {
            ArchiveItem = new HashSet<ArchiveItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArchiveDriveId { get; set; }

        public ArchiveDrive ArchiveDrive { get; set; }
        public Usr User { get; set; }
        public ICollection<ArchiveItem> ArchiveItem { get; set; }
    }
}
