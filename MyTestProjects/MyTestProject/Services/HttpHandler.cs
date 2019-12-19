using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyTestProject.Services
{
  public interface IHttpHandler
  {
    Task<HttpResponseMessage> GetAsync(string url);
  }

  public class MyHttpClientHandler : IHttpHandler
  {
    private HttpClient _client = new HttpClient();

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
      return await _client.GetAsync(url);
    }
  }
}
