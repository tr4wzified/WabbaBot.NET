using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Core
{
    public static class EventHandlers
    {
        public static Task OnReady(DiscordClient sender, ReadyEventArgs e)
        {
            sender.Logger.LogInformation("WabbaBot ready to process events!");

            return Task.CompletedTask;
        }
        public static Task OnClientError(DiscordClient sender, ClientErrorEventArgs e)
        {
            sender.Logger.LogError(e.Exception, "[ClientError]");

            return Task.CompletedTask;
        }
        public static Task OnCommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            e.Context.Client.Logger.LogInformation($"[CommandExecuted] {e.Context.User.Username} ({e.Context.User.Id}): {e.Context.Message.Content}'");

            return Task.CompletedTask;
        }
        public static async Task OnCommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is CommandNotFoundException)
                return;

            e.Context.Client.Logger.LogError($"[CommandError] {e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            var errorPrefix = "**An error occurred!** ";
            if (e.Exception is ChecksFailedException ex)
                await e.Context.RespondAsync($"{errorPrefix} You do not have the permissions required to execute this command.");
            else if (e.Exception is ArgumentException) {
                StringBuilder message = new StringBuilder();
                message.AppendLine($"Too few arguments for command `{e.Command.Name}`!");
                foreach (var overload in e.Command.Overloads)
                {
                    message.Append($"Usage: `{Bot.Settings.Prefixes[0]}{e.Command.Name}");
                    foreach (var argument in overload.Arguments) {
                        message.Append($" <{argument.Name}>");
                    }
                    message.Append("`");
                }
                await e.Context.RespondAsync(message.ToString());
            }
            else
                await e.Context.RespondAsync($"{errorPrefix} {e.Exception.Message}");

        }
    }
}
