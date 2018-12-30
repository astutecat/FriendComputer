using System.Threading.Tasks;
using Discord.Commands;
using FriendComputer.BaseCommands;
using Microsoft.Extensions.Logging;
using FriendComputer;

namespace FriendComputer.Discord.Commands
{
  public class DiscordPingCommand : PingCommand, IDiscordBotCommand
  {
    public DiscordPingCommand(ILogger<PingCommand> logger) : base(logger)
    {
    }

    public async Task ExecuteAsync(ICommandContext ctx, int argPos)
    {
      (bool success, string reply) = GetReply();

      if (success)
      {
        using (ctx.Channel.EnterTypingState())
        {
          var dm_channel = await ctx.Message.Author.GetOrCreateDMChannelAsync();
          await dm_channel.SendMessageAsync(reply);
        }
      }
    }
  }
}
