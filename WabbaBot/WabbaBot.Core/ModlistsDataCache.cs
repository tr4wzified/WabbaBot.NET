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
    public static class ModlistsDataCache
    {

        private static List<Modlist> _modlists;
        public static void Refresh()
        {
            Dictionary<string, List<long>> modlistMaintainerPairs;
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(Settings.ModlistsDataURL);
                _modlists = JsonConvert.DeserializeObject<List<Modlist>>(json);
                modlistMaintainerPairs = JsonConvert.DeserializeObject<Dictionary<string, List<long>>>(File.ReadAllText(Settings.DiscordMaintainersPath));
            }

            foreach (var (modlistId, maintainers) in modlistMaintainerPairs)
            {
                var modlist = _modlists.Find(m => m.Links.Id == modlistId);
                if (modlist != null)
                {
                    modlist.DiscordMaintainerIds = maintainers;
                }
            }
        }
        public static List<Modlist> GetModlists()
        {
            if (_modlists == null)
                Refresh();

            return _modlists;
        }
    }
}
