using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FriendComputer.BaseCommands
{
  public class PingCommand : ICommand
  {
    private readonly ILogger<PingCommand> _logger;

    public PingCommand(ILogger<PingCommand> logger)
    {
      _logger = logger;
    }

    public async Task<(bool, string)> ExecuteAsync()
    {
      return await Task.Run(() => (true, "pong!"));
    }
  }
}
