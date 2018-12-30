using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FriendComputer.BaseCommands;

namespace FriendComputer.Discord.Commands
{
  public class DiscordBunnyCommand : BunnyCommand, IDiscordBotCommand
  {
    public DiscordBunnyCommand(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task ExecuteAsync(ICommandContext ctx, int argPos)
    {
      (bool success, string image) = await GetImageUrlAsync();

      if (success)
      {
        using (ctx.Channel.EnterTypingState())
        {
          var embed = new EmbedBuilder().WithImageUrl(image);

          await ctx.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
        }
      }
    }
  }
}
