using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FriendComputer.BaseCommands
{
  public class FoxCommand : IImageCommand
  {
    private readonly Uri _baseUri = new Uri("https://randomfox.ca/floof/");
    private readonly HttpClient _httpClient;

    public FoxCommand(IHttpClientFactory httpClientFactory)
    {
      _httpClient = httpClientFactory.CreateClient(nameof(FoxCommand));
    }

    public async Task<(bool, string)> GetImageUrlAsync()
    {
      var response = await _httpClient.GetAsync(_baseUri);
      if (!response.IsSuccessStatusCode)
      {
        return (false, null);
      }

      var json = await response.Content.ReadAsStringAsync();
      dynamic data = JObject.Parse(json);
      string image = data?.image;

      return (!string.IsNullOrEmpty(image), image);
    }

    public async Task<(bool, string)> ExecuteAsync()
    {
      return await GetImageUrlAsync();
    }
  }
}
