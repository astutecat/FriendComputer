using Microsoft.Extensions.Logging;

namespace FriendComputer.BaseCommands
{
  public abstract class PingCommand
  {
    private readonly ILogger<PingCommand> _logger;

    public PingCommand(ILogger<PingCommand> logger)
    {
      _logger = logger;
    }

    internal (bool, string) GetReply()
    {
      return (true, "pong!");
    }
  }
}
