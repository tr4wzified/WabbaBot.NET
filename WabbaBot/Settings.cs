using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot
{
    public class Settings
    {
        public string Token;

        public string[] Prefixes = new string[] { "/" };

        public bool EnableDMs = false;
        public bool CaseSensitive = true;
        public string ModlistsDataURL = "https://raw.githubusercontent.com/wabbajack-tools/mod-lists/master/modlists.json";

        public int CacheRefreshInHours = 24;
        public string DiscordMaintainersPath = "./Config/Maintainers.json";
    }
}
