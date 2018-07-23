using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class Destination
    {
        public Destination()
        {
            ArchiveDrive = new HashSet<ArchiveDrive>();
            Drive = new HashSet<Drive>();
            MenuItem = new HashSet<MenuItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string StreetAddress { get; set; }

        public ICollection<ArchiveDrive> ArchiveDrive { get; set; }
        public ICollection<Drive> Drive { get; set; }
        public ICollection<MenuItem> MenuItem { get; set; }
    }
}
