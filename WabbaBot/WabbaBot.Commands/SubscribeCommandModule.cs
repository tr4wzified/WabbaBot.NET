using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WabbaBot.Core;
using WabbaBot.Objects;

namespace WabbaBot.Commands
{
    public class SubscribeCommandModule : BaseCommandModule
    {
        [Command("addmaintainer")]
        public async Task AddMaintainerCommand(CommandContext cc, string modlistId, DiscordMember discordMember)
        {
            await cc.TriggerTypingAsync();

            List<Modlist> modlists = ModlistsDataCache.GetModlists(forceRefresh: true);
            Modlist modlist = modlists.Find(m => m.Links.Id == modlistId);
            if (modlist != null)
            {
                if (modlist.DiscordMaintainerIds.Contains(discordMember.Id))
                    throw new Exception($"User **{discordMember.DisplayName}** is already maintaining **{modlist.Title}**.");

                modlist.DiscordMaintainerIds.Add(discordMember.Id);
                modlists.ToSimplifiedOwnershipDictionary().Save();
                await cc.RespondAsync($"Modlist **{modlist.Title}** is now {(modlist.DiscordMaintainerIds.Count >= 2 ? "additionally maintained" : "solely maintained")} by **{discordMember.DisplayName}**.");
            }
            else throw new NullReferenceException($"Modlist with id **{modlistId}** not found in external modlists json.");
        }

        [Command("delmaintainer")]
        public async Task DelMaintainerCommand(CommandContext cc, string modlistId, DiscordMember discordMember)
        {
            await cc.TriggerTypingAsync();

            List<Modlist> modlists = ModlistsDataCache.GetModlists();
            Modlist modlist = modlists.Find(m => m.Links.Id == modlistId);

            if (modlist != null)
            {
                if (!modlist.DiscordMaintainerIds.Contains(discordMember.Id))
                    throw new Exception($"**{discordMember.DisplayName}** wasn't maintaining **{modlist.Title}** in the first place.");

                modlist.DiscordMaintainerIds.Remove(discordMember.Id);
                modlists.ToSimplifiedOwnershipDictionary().Save();
                if (!modlist.DiscordMaintainerIds.Any())
                    await cc.RespondAsync($"Modlist **{modlist.Title}** is no longer being maintained by anyone.");
                else
                    await cc.RespondAsync($"Modlist **{modlist.Title}** is no longer being maintained by **{discordMember.DisplayName}**.");
            }
            else throw new NullReferenceException($"Modlist with id **{modlistId}** not found.");
        }

        [Command("showmaintainers")]
        public async Task ShowMaintainersCommand(CommandContext cc, string modlistId)
        {
            await cc.TriggerTypingAsync();

            List<Modlist> modlists = ModlistsDataCache.GetModlists();
            Modlist modlist = modlists.Find(m => m.Links.Id == modlistId);

            StringBuilder message = new StringBuilder();
            if (modlist != null)
            {
                message.AppendLine($"Modlist **{modlist.Title}** is being managed by the following people:");

                for (int i = 0; i < modlist.DiscordMaintainerIds.Count; i++)
                {
                    var maintainerId = modlist.DiscordMaintainerIds[i];
                    DiscordMember maintainer = default(DiscordMember);
                    foreach (var guild in Bot.Client.Guilds)
                    {
                        try
                        {
                            maintainer = guild.Value.GetMemberAsync(maintainerId).Result;
                            break;
                        }
                        catch (AggregateException)
                        {
                            continue;
                        }
                    }
                    if (maintainer != default(DiscordMember))
                        message.Append($" **{maintainer.DisplayName}** ({maintainerId})");
                    else
                        message.Append($" *Unknown username.* ({maintainerId})");

                    if (i < modlist.DiscordMaintainerIds.Count - 1)
                        message.AppendLine(",");
                }
                await cc.RespondAsync(message.ToString());
            }
            else throw new NullReferenceException($"Modlist with id **{modlistId}** not found.");
        }

        [Command("subscribe")]
        [Aliases("listen")]
        public async Task SubscribeCommand(CommandContext cc, string modlistId, DiscordChannel channel)
        {
            await cc.TriggerTypingAsync();

            Modlist modlist = ModlistsDataCache.GetModlists().FirstOrDefault(m => m.Links.Id == modlistId);
            if (modlist == default(Modlist))
                throw new Exception($"Modlist with id **{modlistId}** does not exist or does not have any maintainers!");


            bool success = false;
            var server = Bot.SubscribedServers.FirstOrDefault(ss => ss.Id == cc.Guild.Id);

            if (server != null && server != default(SubscribedServer))
            {
                var subscribedChannel = server.SubscribedChannels.FirstOrDefault(sc => sc.Id == channel.Id);
                if (subscribedChannel != null && subscribedChannel != default(SubscribedChannel))
                {
                    if (subscribedChannel.Subscriptions.Contains(modlistId))
                        throw new Exception($"Channel {channel.Mention} is already subscribed to **{modlist.Title}**!");

                    success = subscribedChannel.Subscriptions.Add(modlistId);
                }
                else
                {
                    subscribedChannel = new SubscribedChannel(channel.Id);
                    success = subscribedChannel.Subscriptions.Add(modlistId);
                    server.SubscribedChannels.Add(subscribedChannel);
                }
            }
            else
            {
                server = new SubscribedServer(cc.Guild);
                success = server.SubscribedChannels.Add(new SubscribedChannel(channel.Id, new HashSet<string>() { modlistId }));
                Bot.SubscribedServers.Add(server);
            }

            if (success)
            {
                Bot.SubscribedServers.Save();
                await cc.RespondAsync($"Channel {channel.Mention} will now receive notifications whenever a new version of **{modlist.Title}** releases!");
            }
        }

        [Command("unsubscribe")]
        [Aliases("unlisten")]
        public async Task UnsubscribeCommand(CommandContext cc, string modlistId, DiscordChannel channel)
        {
            await cc.TriggerTypingAsync();

            var subscribedServer = Bot.SubscribedServers.FirstOrDefault(ss => ss.Id == cc.Guild.Id);
            if (subscribedServer != null && subscribedServer != default(SubscribedServer))
            {
                var subscribedChannel = subscribedServer.SubscribedChannels.FirstOrDefault(sc => sc.Id == channel.Id);
                if (subscribedChannel != null && subscribedChannel != default(SubscribedChannel))
                {
                    if (subscribedChannel.Subscriptions.Remove(modlistId))
                    {
                        if (!subscribedChannel.Subscriptions.Any())
                            subscribedServer.SubscribedChannels.Remove(subscribedChannel);

                        if (!subscribedServer.SubscribedChannels.Any())
                            Bot.SubscribedServers.Remove(subscribedServer);

                        Bot.SubscribedServers.Save();
                        await cc.RespondAsync($"Channel {channel.Mention} will no longer receive release notifications of modlist **{ModlistsDataCache.GetModlists().FirstOrDefault(modlist => modlist.Links.Id == modlistId).Title}**!");
                    }
                    else throw new Exception($"Channel {channel.Mention} is not listening to this modlist!");
                }
                else throw new Exception($"Channel {channel.Mention} is not subscribed to any modlists!");
            }
            else throw new Exception("This server is not subscribed to any modlists!");
        }

        [Command("subscriptions")]
        [Aliases("showlisteners")]
        public async Task SubscriptionsCommand(CommandContext cc)
        {
            await cc.TriggerTypingAsync();

            var subscribedServer = Bot.SubscribedServers.FirstOrDefault(ss => ss.Id == cc.Guild.Id);
            if (subscribedServer != null && subscribedServer != default(SubscribedServer))
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine($"Server **{cc.Guild.Name}** is receiving modlist release notifications in {subscribedServer.SubscribedChannels.Count} {(subscribedServer.SubscribedChannels.Count > 1 ? "channels" : "channel")}.");
                var modlists = ModlistsDataCache.GetModlists();
                foreach (var subscribedChannel in subscribedServer.SubscribedChannels)
                {
                    message.Append($"{subscribedChannel.ToDiscordChannel().Mention} will receive release notifications for{(subscribedChannel.Subscriptions.Count > 1 ? " the following lists:" : "")}");
                    int i = 0;
                    foreach (var modlistId in subscribedChannel.Subscriptions)
                    {
                        var modlist = modlists.FirstOrDefault(modlist => modlist.Links.Id == modlistId);

                        if (i == subscribedChannel.Subscriptions.Count - 1)
                            message.Append($" **{modlist.Title}**");
                        else
                            message.Append($" **{modlist.Title}**,");

                        i++;
                    }
                    message.AppendLine(".");
                }
                await cc.RespondAsync(message.ToString());
            }
            else
                throw new Exception("This server is not receiving any release messages at the moment - there are no subscription details to show.");

        }

    }
}
