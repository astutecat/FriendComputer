using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FriendComputer.Discord;
using FriendComputer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FriendComputer
{
  public class Program
  {
    public static async Task InitDbAsync(IServiceProvider serviceProvider,
                                         bool createUsers = true)
    {
      using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
        .CreateScope())
      {
        var db = serviceScope.ServiceProvider.GetService<CarlContext>();

        if (await db.Database.EnsureCreatedAsync())
        {
          await db.Database.MigrateAsync();
        }
      }
    }

    public static async Task Main(string[] args)
    {
      var host = GetHostBuilder(args).UseConsoleLifetime().Build();

      await InitDbAsync(host.Services);
      await host.StartAsync();
      await host.WaitForShutdownAsync();
    }

    internal static IHostBuilder GetHostBuilder(string[] args)
    {
      return new HostBuilder()
      .ConfigureAppConfiguration((hostingContext, config) =>
      {
        config.AddEnvironmentVariables();

        if (args != null)
        {
          config.AddCommandLine(args);
        }
      })
        .ConfigureServices((hostContext, services) =>
        {
          services.AddOptions();
          services.Configure<AppConfig>(hostContext.Configuration);

          services.AddSingleton<IDiscordBot, DiscordBot>();
          services.AddHostedService<DiscordBot>();
          services.AddDbContext<CarlContext>();

          foreach (var t in GetAllTypesOf<IRestApiCommand>())
            {
              services.AddHttpClient(t.Name);
            }

          foreach (var t in GetAllTypesOf<IBotCommand>())
            {
              services.AddScoped(typeof(IBotCommand), t);
            }

          services.AddSingleton<ICommandFactory<IBotCommand>, CommandFactory<IBotCommand>>();
        })
        .ConfigureLogging((hostingContext, logging) =>
        {
          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
          logging.AddConsole();
        });
    }

    private static IEnumerable<Type> GetAllTypesOf<T>()
    {
      var platform = Environment.OSVersion.Platform.ToString();
      var runtimeAssemblyNames = DependencyContext.Default
        .GetRuntimeAssemblyNames(platform);

      var results = runtimeAssemblyNames
          .Select(Assembly.Load)
        .SelectMany(a => a.ExportedTypes)
        .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(T)))
        .ToList();

      return results;
    }
  }
}
