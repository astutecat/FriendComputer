using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FriendComputer.Discord
{
  internal class DiscordBot : IHostedService, IDisposable, IDiscordBot
  {
    private readonly IOptions<AppConfig> _config;
    private CancellationTokenSource _cancellationSource;
    private DiscordSocketClient _client;
    private DiscordSocketConfig _socketConfig;
    private ICommandFactory<ICommand> _commandFactory;
    private ICommandExecutor _commandExecutor;
    private ILogger<DiscordBot> _logger;
    private Task _startupTask;
    private Timer _timer;

    public DiscordBot(IOptions<AppConfig> config,
                      ILogger<DiscordBot> logger,
                      ICommandFactory<ICommand> commandFactory,
                      ICommandExecutor commandExecutor)
    {
      _cancellationSource = new CancellationTokenSource();
      _config = config;
      _logger = logger;
      _client = new DiscordSocketClient();
      _socketConfig = new DiscordSocketConfig()
      {
        MessageCacheSize = _config.Value.MessagesToCache,
      };
      _client.Log += Log;
      _commandFactory = commandFactory;
      _commandExecutor = commandExecutor ?? throw new ArgumentNullException(nameof(commandExecutor));
    }

    public void Dispose()
    {
      _timer.Dispose();
      _client.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _client.MessageReceived += MessageReceivedAsync;
      _startupTask = ConnectAsync();
      return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      _cancellationSource.Cancel();
      await _client.StopAsync();
      _client.MessageReceived -= MessageReceivedAsync;
      _client.Log -= Log;
    }

    internal Task Log(LogMessage msg)
    {
      return Task.Run(() =>
      {
        _logger.Log(ConvertLogLevel(msg.Severity), msg.Exception, msg.Message);
      });
    }

    internal Task MessageReceivedAsync(SocketMessage messageParam)
    {
      return Task.Run(async () =>
      {
        if (!(messageParam is SocketUserMessage message))
        {
          return;
        }
        var argPos = 0;
        if (message.HasCharPrefix('!', ref argPos) ||
          message.HasMentionPrefix(_client.CurrentUser, ref argPos))
        {
          var ctx = new CommandContext(_client, message);
          var command = ctx.Message.Content
            .Substring(argPos)
            .Split(" ")
            .First()
            .ToLowerInvariant();

          var botCommand = _commandFactory.FindOrDefault<ICommand>(command);
          if (botCommand == null) return;

          try
          {
            await _commandExecutor.ExecuteAsync(ctx, argPos, botCommand);
          }
          catch (Exception ex)
          {
            _logger.LogError(ex, "Error executing command");
          }
        }
      });
    }

    private void CheckConnection(object state)
    {
      if (_cancellationSource.Token.IsCancellationRequested)
      {
        _timer?.Change(Timeout.Infinite, 0);
        return;
      }

      switch (_client.ConnectionState)
      {
        case ConnectionState.Disconnected:
          _startupTask = ConnectAsync();
          return;
        case ConnectionState.Connecting:
        case ConnectionState.Connected:
        case ConnectionState.Disconnecting:
          return;
      }
    }

    private async Task ConnectAsync(int failedAttempts = 0)
    {
      try
      {
        await _client.LoginAsync(TokenType.Bot, _config.Value.BotToken);
        await _client.StartAsync();
        _timer = new Timer(CheckConnection,
          null,
          TimeSpan.FromSeconds(10),
          TimeSpan.FromSeconds(5));
      }
      catch (HttpException ex)
      {
        if (failedAttempts < 5)
        {
          _logger.LogError("Discord Login error: {Message}", ex.Message);
          Thread.Sleep(TimeSpan.FromSeconds(10));
          await ConnectAsync(failedAttempts++);
        }
        else
        {
          _logger.LogError("Connection failed after 5 attempts to connect last error was: {Message}",
            ex.Message);
          throw;
        }
      }
    }

    private LogLevel ConvertLogLevel(LogSeverity severity)
    {
      switch (severity)
      {
        case LogSeverity.Critical:
          return LogLevel.Critical;
        case LogSeverity.Error:
          return LogLevel.Error;
        case LogSeverity.Warning:
          return LogLevel.Warning;
        case LogSeverity.Info:
          return LogLevel.Information;
        case LogSeverity.Verbose:
          return LogLevel.Debug;
        case LogSeverity.Debug:
          return LogLevel.Trace;
        default:
          return LogLevel.Information;
      }
    }
  }
}
