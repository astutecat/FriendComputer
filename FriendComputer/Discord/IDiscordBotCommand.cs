using System.Threading.Tasks;
using Discord.Commands;

namespace FriendComputer.Discord
{
  internal interface IDiscordBotCommand : IBotCommand
  {
    Task ExecuteAsync(ICommandContext ctx, int argPos, ICommand command);
    Task ExecuteAsync(ICommandContext ctx, int argPos, IImageCommand command);
  }
}
