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
            var client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Settings.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            client.Ready += EventHandlers.OnReady;
            client.ClientErrored += EventHandlers.OnClientError;

            var commandsNextConfiguration = new CommandsNextConfiguration()
            {
                StringPrefixes = Settings.Prefixes,
                EnableDms = Settings.EnableDMs,
                CaseSensitive = Settings.CaseSensitive
            };

            var commands = client.UseCommandsNext(commandsNextConfiguration);
            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            commands.CommandExecuted += EventHandlers.OnCommandExecuted;
            commands.CommandErrored += EventHandlers.OnCommandErrored;

            await client.ConnectAsync();
            await Task.Delay(-1);
        }

    }
}
