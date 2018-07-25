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
        public User To { get; }
        public string Content { get; }

        public Message(User from, User to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }
    }
}
