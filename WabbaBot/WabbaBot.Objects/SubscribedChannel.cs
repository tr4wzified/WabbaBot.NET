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
        public DiscordChannel Channel { get; }

        public List<Modlist> Subscriptions { get; set; }

        public SubscribedChannel(DiscordChannel channel)
        {
            Channel = channel;
            Subscriptions = new List<Modlist>();
        }

        public SubscribedChannel(DiscordChannel channel, List<Modlist> subscriptions)
        {
            Channel = channel;
            Subscriptions = subscriptions;
        }
    }
}
