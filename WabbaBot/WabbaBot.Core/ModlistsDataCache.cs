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
        private static void Refresh()
        {
            Dictionary<string, List<long>> modlistMaintainerPairs = null;
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(Settings.ModlistsDataURL);
                _modlists = JsonConvert.DeserializeObject<List<Modlist>>(json);
                if (File.Exists(Settings.DiscordMaintainersPath))
                {
                    modlistMaintainerPairs = JsonConvert.DeserializeObject<Dictionary<string, List<long>>>(File.ReadAllText(Settings.DiscordMaintainersPath));
                }
            }

            if (modlistMaintainerPairs != null)
            {
                foreach (var (modlistId, maintainers) in modlistMaintainerPairs)
                {
                    var modlist = _modlists.Find(m => m.Links.Id == modlistId);
                    if (modlist != null)
                    {
                        modlist.DiscordMaintainerIds = maintainers;
                    }
                }
            }
        }
        public static List<Modlist> GetModlists(bool forceRefresh = false)
        {
            if (_modlists == null || forceRefresh)
                Refresh();

            return _modlists;
        }
    }
}
