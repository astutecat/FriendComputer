using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FriendComputer.Discord
{
  internal class DiscordCommandExecutor : ICommandExecutor
  {

    public async Task ExecuteAsync(CommandContext ctx, int argPos, ICommand command)
    {
      (bool success, string text) = await command.ExecuteAsync();

      using (ctx.Channel.EnterTypingState())
      {
        await ctx.Channel.SendMessageAsync(text);
      }
    }

    public async Task ExecuteAsync(CommandContext ctx, int argPos, IImageCommand command)
    {
      (bool success, string image) = await command.GetImageUrlAsync();

      using (ctx.Channel.EnterTypingState())
      {
        var embed = new EmbedBuilder().WithImageUrl(image).Build();
        await ctx.Channel.SendMessageAsync(string.Empty, embed: embed);
      }
    }
  }
}
