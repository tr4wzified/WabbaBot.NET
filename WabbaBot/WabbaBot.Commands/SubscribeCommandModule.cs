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
            else throw new NullReferenceException($"Modlist with id **{modlistId}** not found in external modlists json.");
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
    }
}
