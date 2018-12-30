using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FriendComputer.BaseCommands
{
  public abstract class BunnyCommand : IRestApiCommand
  {
    private readonly Uri _baseUri = new Uri("https://api.bunnies.io/v2/loop/random/?media=gif");
    private readonly HttpClient _httpClient;

    public BunnyCommand(IHttpClientFactory httpClientFactory)
    {
      _httpClient = httpClientFactory.CreateClient(nameof(BunnyCommand));
    }

    internal async Task<(bool, string)> GetImageUrlAsync()
    {
      var response = await _httpClient.GetAsync(_baseUri);
      if (!response.IsSuccessStatusCode)
      {
        return (false, null);
      }

      var json = await response.Content.ReadAsStringAsync();
      dynamic data = JObject.Parse(json);
      var image = data?.media?.gif;

      return (!string.IsNullOrEmpty(image), image);
    }
  }
}
