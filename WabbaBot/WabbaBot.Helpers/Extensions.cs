using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WabbaBot.Objects;

namespace WabbaBot.Core
{
    public static class Extensions
    {
        public static DiscordGuild ToDiscordGuild(this SubscribedServer server) => Bot.Client.GetGuildAsync(server.Id).Result;
        public static DiscordChannel ToDiscordChannel(this SubscribedChannel channel) => Bot.Client.GetChannelAsync(channel.Id).Result;
        public static Dictionary<string, List<ulong>> ToSimplifiedOwnershipDictionary(this List<Modlist> modlists) => modlists.ToDictionary(m => m.Links.Id, m => m.DiscordMaintainerIds);
        public static void Save(this Dictionary<string, List<ulong>> simplifiedOwnershipDictionary) => File.WriteAllText(Bot.Settings.DiscordMaintainersPath, JsonConvert.SerializeObject(simplifiedOwnershipDictionary, Formatting.Indented));
        public static void Save(this HashSet<SubscribedServer> subscribedServers) => File.WriteAllText(Bot.Settings.SubscribedServersPath, JsonConvert.SerializeObject(subscribedServers, Formatting.Indented));
    }
}
