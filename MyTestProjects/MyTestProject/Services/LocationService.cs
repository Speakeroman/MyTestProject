using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Web;
using MyTestProject.DTO;
using System.Linq;
using System.Globalization;

namespace MyTestProject.Services
{
  public interface ILocationService
  {
    Task<Location> GetGeocodeJsone(string addressText);
    Task<GoogleTimeZone> GetTimeZoneJsone(Location location, TimeSpan dateTimeSpan);
  }

  public class LocationService : ILocationService
  {
    public static IConfiguration _configuration { get; private set; }
    private static ILogger _logger;
    private readonly IHttpHandler _client;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    public LocationService(ILogger logger, IConfiguration configuration, IHttpHandler client)
    {
      _client = client;
      _configuration = configuration;
      _logger = logger;
      _baseUrl = configuration.GetSection("GoogleApiUrl").GetSection("googleUrl")?.Value;
      _apiKey = configuration.GetSection("GoogleApiUrl").GetSection("key")?.Value;
    }

    public async Task<Location> GetGeocodeJsone(string addressText)
    {
      var encodedAddress = HttpUtility.UrlEncode(addressText);
      HttpResponseMessage httpResponse = await _client.GetAsync($"{_baseUrl}/geocode/json?address={encodedAddress}&key={_apiKey}");

      if (!httpResponse.IsSuccessStatusCode)
      {
        _logger.LogError("");
        throw new Exception("Cannot retrieve tasks");
      }

      var content = await httpResponse.Content.ReadAsStringAsync();
      GeocodeResponse deserializedResponce = JsonConvert.DeserializeObject<GeocodeResponse>(content);
      Location result = null;
      if (deserializedResponce.Status == "OK" && deserializedResponce.Results.Length != 0)
      {
        result = new Location();
        result.Latitude = deserializedResponce.Results.Select(x => x.Geometry.Location.Lat).FirstOrDefault();
        result.Longitude = deserializedResponce.Results.Select(x => x.Geometry.Location.Lng).FirstOrDefault();

        var country = deserializedResponce.Results.Select(x => x.AddressComponents.Where(y => y.Types.Contains("country"))).FirstOrDefault().FirstOrDefault().ShortName;
        
        var cultureInfosList = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => c.Name.EndsWith("-" + country));
        result.CountryCode = country != "GB"? cultureInfosList.FirstOrDefault().ToString() : cultureInfosList.LastOrDefault().ToString();

        result.FormattedAddress = deserializedResponce.Results.Select(x => x.FormatedAddress).FirstOrDefault();

        var utcTimeNow = DateTime.UtcNow;
        TimeSpan utcDateNowSpan = utcTimeNow - new DateTime(1970, 1, 1, 0, 0, 0);
        var googleTimeZoneResponce = this.GetTimeZoneJsone(result, utcDateNowSpan).Result;
        if (googleTimeZoneResponce.Status == "OK")
        {
          result.GoogleTimeZone = googleTimeZoneResponce;
        }
      }
      return result;
    }

    public async Task<GoogleTimeZone> GetTimeZoneJsone(Location location, TimeSpan dateTimeSpan)
    {
      HttpResponseMessage httpResponse = await _client.GetAsync($"{_baseUrl}/timezone/json?" +
          $"location={location.Latitude.ToString().Replace(",", ".")},{location.Longitude.ToString().Replace(",", ".")}" +
          $"&timestamp={dateTimeSpan.TotalSeconds.ToString().Replace(",", ".")}&key={_apiKey}");

      if (!httpResponse.IsSuccessStatusCode)
      {
        _logger.LogError("");
        throw new Exception("Cannot retrieve tasks");
      }

      var content = await httpResponse.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<GoogleTimeZone>(content);

      return result;
    }
  }
}
