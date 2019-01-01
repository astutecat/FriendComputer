using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace FriendComputer
{
  interface ICommandExecutor
  {
    Task ExecuteAsync(CommandContext ctx, int argPos, ICommand command);
    Task ExecuteAsync(CommandContext ctx, int argPos, IImageCommand command);
  }
}
