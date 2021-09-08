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
                {
                    throw new Exception($"**{discordMember.DisplayName}** is already maintaining **{modlist.Title}");
                }

                modlist.DiscordMaintainerIds.Add(discordMember.Id);
                modlists.ToSimplifiedOwnershipDictionary().SaveToJson(Bot.Settings.DiscordMaintainersPath);
                await cc.RespondAsync($"Modlist **{modlist.Title}** is now maintained by by **{discordMember.DisplayName}** was added to the database.");
            }
            else
                throw new NullReferenceException($"Modlist with id **{modlistId}** not found in external modlists json.");
        }

        [Command("listen")]
        public async Task ListenCommand(CommandContext cc, string modlistId, DiscordChannel channel)
        {
            await cc.RespondAsync($"Now listening to **{modlistId}** in {channel.Name}.");
        }
    }
}
