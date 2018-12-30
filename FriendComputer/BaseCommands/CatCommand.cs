using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FriendComputer.BaseCommands
{
  public abstract class CatCommand : IRestApiCommand
  {
    private readonly Uri _baseUri = new Uri("https://aws.random.cat/meow");
    private readonly HttpClient _httpClient;

    public CatCommand(IHttpClientFactory httpClientFactory)
    {
      _httpClient = httpClientFactory.CreateClient(nameof(CatCommand));
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
      string image = data?.file;

      return !string.IsNullOrEmpty(image) ? (true, image) : (false, null);
    }
  }
}
