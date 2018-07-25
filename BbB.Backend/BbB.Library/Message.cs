using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class Message
    {
        public User From { get; }
        public string Content { get; }

        public Message(User from, string content)
        {
            From = from;
            Content = content;
        }
    }
}
