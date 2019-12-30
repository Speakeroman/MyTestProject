using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTestProject.DTO;
using MyTestProject.Services;

namespace MyTestProject.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AddressController : ControllerBase
  {
    private readonly ILogger<AddressController> _logger;
    private readonly IConfiguration _config;
    private readonly ILocationService _locationService;

    public AddressController(ILogger<AddressController> logger, IConfiguration config, ILocationService locationService)
    {
      _logger = logger;
      _config = config;
      _locationService = locationService;
    }

    [HttpPost]
    public Location Post(AddressRequest addressText)
    {
      Location result = _locationService.GetGeocodeJsone(addressText.LocationAddress).Result;
      if (result != null)
      {
        _logger.LogInformation(result.ToString());
      }
      else
      {
        _logger.LogWarning("No results");
      }

      return result;
    }
  }
}
