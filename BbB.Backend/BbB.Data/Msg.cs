using System;
using System.Collections.Generic;

namespace BbB.Data
{
    public partial class Msg
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime? Dtime { get; set; }
        public string Msg1 { get; set; }

        public Usr Receiver { get; set; }
        public Usr Sender { get; set; }
    }
}
