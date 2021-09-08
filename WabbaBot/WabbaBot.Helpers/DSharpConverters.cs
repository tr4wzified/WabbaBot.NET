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
        public static void SaveToJson(this Dictionary<string, List<ulong>> simplifiedOwnershipDictionary, string path) => File.WriteAllText(path, JsonConvert.SerializeObject(simplifiedOwnershipDictionary, Formatting.Indented));
    }
}
