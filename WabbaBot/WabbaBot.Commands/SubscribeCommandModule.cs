using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Commands
{
    public class SubscribeCommandModule : BaseCommandModule
    {
        [Command("listen")]
        public async Task ListenCommand(CommandContext cc, string modlistId, DiscordChannel channel)
        {
            await cc.RespondAsync($"Now listening to **{modlistId}** in {channel.Name}.");
        }
    }
}
