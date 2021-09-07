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
        internal long Size { get; set; }

        [DataMember]
        internal long NumberOfArchives { get; set; }

        [DataMember]
        internal long SizeOfArchives { get; set; }

        [DataMember]
        internal long NumberOfInstalledFiles { get; set; }

        [DataMember]
        internal long SizeOfInstalledFiles { get; set; }
    }
}
