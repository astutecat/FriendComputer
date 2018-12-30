using System.Linq;

namespace FriendComputer
{
  internal interface ICommandFactory<T>
  {
    ILookup<string, T> CommandLookup { get; }

    T FindOrDefault(string name);

    U FindOrDefault<U>(string name) where U : T;
  }
}
