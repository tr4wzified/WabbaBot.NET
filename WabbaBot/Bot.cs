using DSharpPlus;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WabbaBot.Core;
using WabbaBot.Objects;

namespace WabbaBot
{
    public class Bot
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextConfiguration CommandsConfiguration { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        public static Settings Settings { get; private set; }
        public static bool IsRunning { get; private set; }
        public static HashSet<SubscribedServer> SubscribedServers { get; private set; }

        public Bot(Settings settings)
        {
            Settings = settings;

            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Settings.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            Client.Ready += EventHandlers.OnReady;
            Client.ClientErrored += EventHandlers.OnClientError;

            if (File.Exists(Settings.SubscribedServersPath))
                SubscribedServers = JsonConvert.DeserializeObject<HashSet<SubscribedServer>>(File.ReadAllText(Settings.SubscribedServersPath));
            else
                SubscribedServers = new HashSet<SubscribedServer>();

            CommandsConfiguration = new CommandsNextConfiguration()
            {
                StringPrefixes = Settings.Prefixes,
                EnableDms = Settings.EnableDMs,
                CaseSensitive = Settings.CaseSensitive
            };

            Commands = Client.UseCommandsNext(CommandsConfiguration);
            Commands.RegisterCommands(Assembly.GetExecutingAssembly());

            Commands.CommandExecuted += EventHandlers.OnCommandExecuted;
            Commands.CommandErrored += EventHandlers.OnCommandErrored;
        }
        public async Task Run()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                await Client.ConnectAsync();
            }
        }

        public async Task Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                await Client.DisconnectAsync();
            }
        }

    }
}
