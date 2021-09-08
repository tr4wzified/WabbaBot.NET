using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WabbaBot.Objects;

namespace WabbaBot.WabbaBot.Core
{
    public static class DSharpPlusConverters
    {
        public static DiscordGuild ToDiscordGuild(this SubscribedServer server) => Bot.Client.GetGuildAsync(server.Id).Result;
        public static DiscordChannel ToDiscordChannel(this SubscribedChannel channel) => Bot.Client.GetChannelAsync(channel.Id).Result;
    }
}
