using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Data;
using LiteDB;

namespace DiscordBot.Modules
{
    [Group("points")]
    public class PointsModule : ModuleBase<SocketCommandContext>
    {
        // This property will be filled in at runtime by the IoC container (Program.cs:49)
        public LiteDatabase Database { get; set; }

        // Access the current user's points, under '!points' or '!points me'
        [Command, Alias("me")]
        public Task SelfAsync()
            => SendPointsAsync(Context.User);

        // Access another user's points, under '!points @user'
        [Command]
        public Task UserAsync(IUser user)
            => SendPointsAsync(user);

        private async Task SendPointsAsync(IUser user)
        {
            var users = Database.GetCollection<User>("users");
            var model = users.FindOne(u => u.Id == user.Id);

            var points = model?.Points ?? 0;

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = user.ToString(),
                    IconUrl = user.GetAvatarUrl()
                },
                Color = new Color(0x346B82), // color of the day
            };
            embed.AddField("Points", points);
            // Levels?

            await ReplyAsync("", embed: embed);
        }
    }
}
