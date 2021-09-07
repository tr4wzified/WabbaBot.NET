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
        [Command("addmodlist")]
        public async Task AddModlistCommand(CommandContext cc, string modlistId, DiscordMember member)
        {
            
            await cc.TriggerTypingAsync();

            ModlistsDataCache.Refresh();
            List<Modlist> modlists = ModlistsDataCache.GetModlists();
            Modlist modlist = modlists.Find(m => m.Links.Id == modlistId);
            if (modlist != null)
            {
                await cc.RespondAsync($"Modlist **{modlist.Title}** managed by **{member.DisplayName}** was added to the database.");
            }
            else
            {
                await cc.RespondAsync($"**An error occurred**! Modlist with id **{modlistId}** not found in external modlists json.");
            }
        }

        [Command("listen")]
        public async Task ListenCommand(CommandContext cc, string modlistId, DiscordChannel channel)
        {
            await cc.RespondAsync($"Now listening to **{modlistId}** in {channel.Name}.");
        }
    }
}
