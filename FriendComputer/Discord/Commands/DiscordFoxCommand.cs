using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FriendComputer.BaseCommands;
using FriendComputer.Discord;
using FriendComputer;

namespace FriendComputer.Discord.Commands
{
  public class DiscordFoxCommand : FoxCommand, IDiscordBotCommand
  {
    public DiscordFoxCommand(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task ExecuteAsync(ICommandContext ctx, int argPos)
    {
      (bool success, string image) = await TryGetImageUrlAsync();

      using (ctx.Channel.EnterTypingState())
      {
        var embed = new EmbedBuilder().WithImageUrl(image).Build();
        await ctx.Channel.SendMessageAsync(string.Empty, embed: embed);
      }
    }
  }
}
