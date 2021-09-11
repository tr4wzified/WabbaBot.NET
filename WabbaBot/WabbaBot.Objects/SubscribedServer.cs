using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Objects
{
    [DataContract]
    public class SubscribedServer
    {
        [DataMember]
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [DataMember]
        [JsonProperty("listening_channels")]
        public HashSet<SubscribedChannel> SubscribedChannels { get; set; }

        [DataMember]
        [JsonProperty("list_roles")]
        public Dictionary<string, ulong> SubscriptionPingRoles { get; set; }
        public SubscribedServer()
        {
            SubscribedChannels = new HashSet<SubscribedChannel>();
            SubscriptionPingRoles = new Dictionary<string, ulong>();
        }
        public SubscribedServer(DiscordGuild guild)
        {
            Id = guild.Id;
            SubscribedChannels = new HashSet<SubscribedChannel>();
            SubscriptionPingRoles = new Dictionary<string, ulong>();
        }
        public void SetPingRole(Modlist modlist, DiscordRole role)
        {
            if (GetSubscriptions().Contains(modlist.Links.Id))
            {
                SubscriptionPingRoles[modlist.Links.Id] = role.Id;
            }
            else
                throw new NullReferenceException($"There are no channels listening to {modlist.Title}.");
        }
        public HashSet<string> GetSubscriptions() => SubscribedChannels.SelectMany(c => c.Subscriptions).Distinct().ToHashSet();

        public static bool operator ==(SubscribedServer a, SubscribedServer b) => a.Id == b.Id;
        public static bool operator !=(SubscribedServer a, SubscribedServer b) => a.Id != b.Id;
    }
}
