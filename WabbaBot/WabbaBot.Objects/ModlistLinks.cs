using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.WabbaBot.Objects
{
    [DataContract]
    public class ModlistLinks
    {
        [DataMember]
        [JsonProperty("image")]
        internal string ImageURL { get; set; }

        [DataMember]
        [JsonProperty("readme")]
        internal string ReadmeURL { get; set; }

        [DataMember]
        [JsonProperty("download")]
        internal string DownloadURL { get; set; }

        [DataMember]
        [JsonProperty("machineURL")]
        internal string Id { get; set; }
    }
}
