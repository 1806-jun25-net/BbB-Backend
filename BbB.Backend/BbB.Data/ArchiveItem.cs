using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class ArchiveItem
    {
        public int Id { get; set; }
        public int ArchiveOrderId { get; set; }
        public string ItemName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Cost { get; set; }
        public string Msg { get; set; }

        public ArchiveOrder ArchiveOrder { get; set; }
    }
}
