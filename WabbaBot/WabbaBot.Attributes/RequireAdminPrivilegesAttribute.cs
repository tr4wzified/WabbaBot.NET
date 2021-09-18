using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireAdminPrivilegesAttribute : CheckBaseAttribute
    {
        //public RequireAdminPrivilegesAttribute() { }
        public override Task<bool> ExecuteCheckAsync(CommandContext cc, bool help) => Task.FromResult(CheckAdminPrivileges(cc));
        public bool CheckAdminPrivileges(CommandContext cc) => Bot.Settings.Administrators.Contains(cc.Member.Id);
    }
}
