using DSharpPlus;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using WabbaBot.Commands;
using WabbaBot.Core;

namespace WabbaBot
{
    class Program
    {
        static void Main() => MainAsync().GetAwaiter().GetResult();
        internal static async Task MainAsync()
        {
            string configFolderPath = @".\Config";
            string settingsPath = Path.Combine(configFolderPath, "Settings.json");
            Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsPath));

            Bot bot = new Bot(settings);
            await bot.Run();

            await Task.Delay(-1);
        }
    }
}
