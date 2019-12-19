using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTestProject.Utils;
using Newtonsoft.Json.Linq;
using MyTestProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyTestProject.Services
{
  public interface ILocationService
  {
    Task<Location> GetGeocodeJsone(string addressText);
    Task<GoogleTimeZone> GetTimeZoneJsone(Location location, TimeSpan dateTimeSpan);
  }

  [Injectable(LifetimeScope.Singleton)]
  public class LocationService : ILocationService
  {
    public static IConfiguration _configuration { get; private set; }
    private static ILogger _logger;
    private readonly IHttpHandler _client;
    private string _baseUrl;
    private string _apiKey;

    public LocationService(ILogger logger, IConfiguration configuration, IHttpHandler client)
    {
      _client = client;
      _configuration = configuration;
      _logger = logger;
      _baseUrl = configuration.GetSection("GoogleApiUrl").GetSection("googleUrl").Value;
      _apiKey = configuration.GetSection("GoogleApiUrl").GetSection("key").Value;
    }

    public async Task<Location> GetGeocodeJsone(string addressText)
    {
      var encodedAddress = TextUtils.EncodeText(addressText);
      HttpResponseMessage httpResponse = await _client.GetAsync($"{_baseUrl}/geocode/json?address={encodedAddress}&key={_apiKey}");

      if (!httpResponse.IsSuccessStatusCode)
      {
        _logger.LogError("");
        throw new Exception("Cannot retrieve tasks");
      }

      var content = await httpResponse.Content.ReadAsStringAsync();
      JObject rss = JObject.Parse(content);
      Location result = new Location();
      result.FormattedAddress = "No results.";
      result.Latitude = 0;
      result.Longitude = 0;
      if (rss["results"].HasValues)
      {
        result.Latitude = (double)rss["results"][0]["geometry"]["location"]["lat"];
        result.Longitude = (double)rss["results"][0]["geometry"]["location"]["lng"];
        result.CountryCode = (string)rss["results"][0]["address_components"].Last["short_name"];
        result.FormattedAddress = (string)rss["results"][0]["formatted_address"];
        result.CountryCode = GetContryFromGoogleResponce(rss);
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

    private static string GetContryFromGoogleResponce(JObject rss)
    {
      string countryName = string.Empty;
      IEnumerable<JToken> short_name = rss.SelectTokens("$.results[0].address_components").FirstOrDefault();
      var jResult = rss["results"][0];
      var jAddressComponents = jResult["address_components"];
      foreach (var jAddressComponent in jAddressComponents)
      {
        var jTypes = jAddressComponent["types"];
        if (jTypes.Any(t => t.ToString() == "country"))
        {
          countryName = jAddressComponent["short_name"].ToString();
        }
      }

      return countryName;
    }
  }
}
