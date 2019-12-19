using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTestProject.Models;
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
            string locationAddress = addressText.locationAddress;
            var utcTimeNow = DateTime.UtcNow;
            TimeSpan utcDateNowSpan = utcTimeNow - new DateTime(1970, 1, 1, 0, 0, 0);
            Location result = _locationService.GetGeocodeJsone(locationAddress).Result;
            var timeZone = _locationService.GetTimeZoneJsone(result, utcDateNowSpan).Result;
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => c.Name.EndsWith("-" + result.CountryCode)).FirstOrDefault();
            result.googleTimeZone = timeZone;
            result.CountryCode = cultureInfos.ToString();

            _logger.LogInformation(result.ToString());

            return result;
        }
    }
}
