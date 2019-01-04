using FluentAssertions;
using FriendComputer;
using FriendComputer.BaseCommands;
using FriendComputer.Discord;
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
      var factory = hostbuilder.Services.GetService<ICommandFactory<ICommand>>();
      var match = factory.FindOrDefault("ping");
      Assert.NotNull(match);
    }

    [Fact]
    public void Test2()
    {
      var hostbuilder = Program.GetHostBuilder(new string[] { }).Build();
      var factory = hostbuilder.Services.GetService<ICommandFactory<ICommand>>();
      var match = factory.FindOrDefault<ICommand>("ping");
      Assert.NotNull(match);
      match.Should().BeOfType<PingCommand>();
    }
  }
}
