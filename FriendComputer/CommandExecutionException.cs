using System;
using System.Collections.Generic;
using System.Text;

namespace FriendComputer
{
  class CommandExecutionException : Exception
  {
    public CommandExecutionException() : base() { }

    public CommandExecutionException(ICommand command) : base(command.ToString()) { }

  }
}
