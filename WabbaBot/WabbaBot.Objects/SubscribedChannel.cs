using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Objects
{
    public class SubscribedChannel
    {
        public ulong Id { get; set; }
        public List<Modlist> Subscriptions { get; set; }

        public SubscribedChannel(ulong channelId)
        {
            Id = channelId;
            Subscriptions = new List<Modlist>();
        }

        public SubscribedChannel(ulong channelId, List<Modlist> subscriptions)
        {
            Id = channelId;
            Subscriptions = subscriptions;
        }

    }
}
