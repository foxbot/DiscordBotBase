using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class DMHandlingModule : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public Task Hello() => ReplyAsync($"Hello, you've slid into my DMs");

    }
}
