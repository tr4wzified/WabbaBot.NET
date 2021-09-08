using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Objects
{
    public class SubscribedServer
    {
        public ulong Id { get; set; }
        public List<SubscribedChannel> SubscribedChannels { get; set; }
        public SubscribedServer()
        {
        }
    }
}
