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
    static class Settings
    {
        public static string Token;

        public static string[] Prefixes = new string[] { "/" };

        public static bool EnableDMs = false;
        public static bool CaseSensitive = true;
        public static string ModlistsDataURL = "https://raw.githubusercontent.com/wabbajack-tools/mod-lists/master/modlists.json";

        public static int CacheRefreshInHours = 24;
        public static string DiscordMaintainersPath = "./Config/Maintainers.json";
    }
}
