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
        static void Main(string[] args)
        {
            string configFolderPath = @".\Config";
            string settingsPath = Path.Combine(configFolderPath, "Settings.json");
            Settings settings;
            using (StreamReader file = File.OpenText(settingsPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                settings = (Settings)serializer.Deserialize(file, typeof(Settings));
            }
            MainAsync(settings).GetAwaiter().GetResult();
        }

        internal static async Task MainAsync(Settings settings)
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = settings.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var commandsNextConfiguration = new CommandsNextConfiguration()
            {
                StringPrefixes = settings.Prefixes,
                EnableDms = settings.EnableDMs,
                CaseSensitive = settings.CaseSensitive
            };

            var modlistsDataCache = new ModlistsDataCache();

            var commands = discord.UseCommandsNext(commandsNextConfiguration);
            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }   
    }
}
