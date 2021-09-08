using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
                modlists.ToSimplifiedOwnershipDictionary().SaveToJson(Bot.Settings.DiscordMaintainersPath);
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
                modlists.ToSimplifiedOwnershipDictionary().SaveToJson(Bot.Settings.DiscordMaintainersPath);
                if (!modlist.DiscordMaintainerIds.Any())
                    await cc.RespondAsync($"Modlist **{modlist.Title}** is no longer being maintained by anyone.");
                else
                    await cc.RespondAsync($"Modlist **{modlist.Title}** is no longer being maintained by **{discordMember.DisplayName}**.");
            }
            else throw new NullReferenceException($"Modlist with id **{modlistId}** not found.");
        }

        [Command("delmaintainer")]
        public async Task DelMaintainerCommand(CommandContext cc, string text)
        {
            await cc.TriggerTypingAsync();

            if (text.ToLower() == "all")
            {
                // Ask to react with yes/no emoji to confirm deletion of all maintainers
            }
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
                    //var maintainer = Bot.Client.Guilds.Values.SelectMany(x => x.Members).FirstOrDefault(x => x.Value.Id == maintainerId).Value;
                    foreach (var guild in Bot.Client.Guilds)
                    {
                        try
                        {
                            maintainer = guild.Value.GetMemberAsync(maintainerId).Result;
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
    }
}
