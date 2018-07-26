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
        public int FromId { get; }
        public int ToId { get; }
        public string Content { get; }

        public Message(int from, int to, string content, int id = -1)
        {
            Id = id;
            FromId = from;
            ToId = to;
            Content = content;
        }
    }
}
