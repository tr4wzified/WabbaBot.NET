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
    public class ModlistDownloadMetadata
    {
        [DataMember]
        internal string Hash { get; set; }

        [DataMember]
        internal ulong Size { get; set; }

        [DataMember]
        internal ulong NumberOfArchives { get; set; }

        [DataMember]
        internal ulong SizeOfArchives { get; set; }

        [DataMember]
        internal ulong NumberOfInstalledFiles { get; set; }

        [DataMember]
        internal ulong SizeOfInstalledFiles { get; set; }
    }
}
