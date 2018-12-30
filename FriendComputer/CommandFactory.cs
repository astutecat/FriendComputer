using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace FriendComputer
{
  public class CommandFactory<T> : ICommandFactory<T>
  {
    private Dictionary<T, IEnumerable<Type>> _interfaceCache;

    public CommandFactory(IServiceProvider provider)
    {
      _interfaceCache = new Dictionary<T, IEnumerable<Type>>();
      CommandLookup = provider.GetServices<T>()
        .ToLookup(a => a.GetType().BaseType.Name.ToLowerInvariant());
    }

    public ILookup<string, T> CommandLookup { get; }

    public T FindOrDefault(string name)
    {
      return CommandLookup.Contains($"{name}command")
        ? CommandLookup[$"{name}command"].FirstOrDefault()
        : default;
    }

    public U FindOrDefault<U>(string name)
      where U : T
    {
      var key = $"{name}command";

      return CommandLookup.Contains(key)
        ? (U)CommandLookup[key].FirstOrDefault(c => ContainsInterface<U>(c))
        : default;
    }

    private bool ContainsInterface<U>(T item)
      where U : T
    {
      if (_interfaceCache.TryGetValue(item, out var value))
      {
        return value.Contains(typeof(U));
      }
      else
      {
        var interfaces = item.GetType().GetInterfaces();
        _interfaceCache.Add(item, interfaces);
        return interfaces.Contains(typeof(U));
      }
    }
  }
}
