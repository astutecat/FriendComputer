
using System.Threading.Tasks;

namespace FriendComputer
{
  internal interface IImageCommand : ICommand
  {
    Task<(bool, string)> GetImageUrlAsync();
  }
}
