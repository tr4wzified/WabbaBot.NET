using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WabbaBot.WabbaBot.Objects;

namespace WabbaBot.Objects
{
    [DataContract]
    public class Modlist
    {
        [DataMember]
        [JsonProperty("title")]
        public string Title { get; set; }

        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DataMember]
        [JsonProperty("author")]
        public string Author { get; set; }

        [DataMember]
        [JsonProperty("maintainers")]
        public List<string> Maintainers { get; set; }

        [DataMember]
        [JsonProperty("game")]
        public string Game { get; set; }

        [DataMember]
        [JsonProperty("official")]
        public bool IsOfficial { get; set; } = true;

        [DataMember]
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [DataMember]
        [JsonProperty("nsfw")]
        public bool IsNSFW { get; set; } = false;

        [DataMember]
        [JsonProperty("utility_list")]
        public bool IsUtilityList { get; set; } = false;

        [DataMember]
        [JsonProperty("force_down")]
        public bool IsForcedDown { get; set; } = false;

        [DataMember]
        [JsonProperty("links")]
        public ModlistLinks Links { get; set; }

        [DataMember]
        [JsonProperty("download_metadata")]
        public ModlistDownloadMetadata DownloadMetadata { get; set; }

        [DataMember]
        [JsonProperty("discord_maintainer_ids")]
        public List<long> DiscordMaintainerIds { get; set; } = new List<long>();

    }
}
