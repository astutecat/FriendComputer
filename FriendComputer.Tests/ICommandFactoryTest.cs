using FluentAssertions;
using FriendComputer;
using FriendComputer.Discord;
using FriendComputer.Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Carl.Tests
{
  public class ICommandFactoryTest
  {
    [Fact]
    public void Test1()
    {
      var hostbuilder = Program.GetHostBuilder(new string[] { }).Build();
      var factory = hostbuilder.Services.GetService<ICommandFactory<IBotCommand>>();
      var match = factory.FindOrDefault("ping");
      Assert.NotNull(match);
    }

    [Fact]
    public void Test2()
    {
      var hostbuilder = Program.GetHostBuilder(new string[] { }).Build();
      var factory = hostbuilder.Services.GetService<ICommandFactory<IBotCommand>>();
      var match = factory.FindOrDefault<IDiscordBotCommand>("ping");
      Assert.NotNull(match);
      match.Should().BeOfType<DiscordPingCommand>();
    }
  }
}
