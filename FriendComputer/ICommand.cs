using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendComputer
{
  internal interface ICommand
  {
    Task<(bool, string)> ExecuteAsync();
  }
}
