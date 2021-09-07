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
using WabbaBot.Helpers;

namespace WabbaBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string configFolderPath = @".\Config";
            string settingsPath = Path.Combine(configFolderPath, "Settings.json");
            StaticJsonDeserializer.Deserialize(File.ReadAllText(settingsPath), typeof(Settings));
            MainAsync().GetAwaiter().GetResult();
        }

        internal static async Task MainAsync()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Settings.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var commandsNextConfiguration = new CommandsNextConfiguration()
            {
                StringPrefixes = Settings.Prefixes,
                EnableDms = Settings.EnableDMs,
                CaseSensitive = Settings.CaseSensitive
            };

            var commands = discord.UseCommandsNext(commandsNextConfiguration);
            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            var modlists = ModlistsDataCache.GetModlists();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }   
    }
}
