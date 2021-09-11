﻿using DSharpPlus;
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
    public class SubscribedChannel
    {
        [DataMember]
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [DataMember]
        [JsonProperty("listening_to")]
        public HashSet<string> Subscriptions { get; set; }

        public SubscribedChannel(ulong channelId)
        {
            Id = channelId;
            Subscriptions = new HashSet<string>();
        }

        public SubscribedChannel(ulong channelId, List<string> subscriptions)
        {
            Id = channelId;
            Subscriptions = subscriptions;
        }
        public static bool operator ==(SubscribedChannel a, SubscribedChannel b) => a.Id == b.Id;
        public static bool operator !=(SubscribedChannel a, SubscribedChannel b) => a.Id != b.Id;

    }
}
