using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class Message
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public Message(int from, int to, string content, DateTime time, int id = -1)
        {
            Id = id;
            FromId = from;
            ToId = to;
            Content = content;
            Time = time;
        }
    }
}
