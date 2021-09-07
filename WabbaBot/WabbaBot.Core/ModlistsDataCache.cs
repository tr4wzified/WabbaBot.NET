using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WabbaBot.Objects;

namespace WabbaBot.Core
{
    public class ModlistsDataCache
    {
        public string ModlistsDataURL { get; set; } = "https://raw.githubusercontent.com/wabbajack-tools/mod-lists/master/modlists.json";
        public List<Modlist> Modlists { get; set; }
        public ModlistsDataCache()
        {
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(ModlistsDataURL);
                Modlists = JsonConvert.DeserializeObject<List<Modlist>>(json);
            }
        }
        public ModlistsDataCache(string modlistsDataURL)
        {
            ModlistsDataURL = modlistsDataURL;
        }
    }
}
