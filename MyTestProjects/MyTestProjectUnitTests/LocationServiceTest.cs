using NUnit.Framework;
using Microsoft.Extensions.Logging;
using MyTestProject.Controllers;
using Moq;
using MyTestProject.Services;
using Microsoft.Extensions.Configuration;
using MyTestProject.DTO;
using System;
using System.Threading.Tasks;

namespace MyTestProjectUnitTests
{
  public class LocationServiceTest
  {
    private Mock<ILogger<AddressController>> _logger;
    private Mock<ILocationService> _locationService;
    private Mock<IConfiguration> _config;
    private AddressController _addressController;
    private Mock<IConfigurationSection> _mockConfSection;

    [SetUp]
    public void Setup()
    {
      // Arrange
      _logger = new Mock<ILogger<AddressController>>();
      _mockConfSection = new Mock<IConfigurationSection>();
      _mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "googleUrl")]).Returns("testurl");
      _mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "key")]).Returns("testKey");
      _config = new Mock<IConfiguration>();
      _config.Setup(a => a.GetSection(It.Is<string>(s => s == "GoogleApiUrl"))).Returns(_mockConfSection.Object);
      _locationService = new Mock<ILocationService>();
      _locationService.Setup(x => x.GetGeocodeJsone(It.IsAny<string>())).ReturnsAsync(new Location() { CountryCode = "UA", Latitude = 0, Longitude = 0, FormattedAddress = "" });
      _locationService.Setup(y => y.GetTimeZoneJsone(It.IsAny<Location>(), It.IsAny<TimeSpan>())).ReturnsAsync(new GoogleTimeZone() { });
      _addressController = new AddressController(_logger.Object, _config.Object, _locationService.Object);
    }

    [Test]
    public void Location_Service_GetGeocodeJsone_Should()
    {
      // Arrange
      var testString = "Lviv";
      // Act
      var response = _locationService.Object.GetGeocodeJsone(testString);

      // Assert
      Assert.IsInstanceOf<Task<Location>>(response);
      Assert.Pass();
    }

    [Test]
    public void LocationService_GetGeocodeJsone()
    {
      // Arrange
      _locationService.Setup(x => x.GetGeocodeJsone(It.IsAny<string>()))
        .ReturnsAsync(new Location()
        {
          CountryCode = "uk-UA",
          Latitude = 49.839683,
          Longitude = 24.029717,
          FormattedAddress = "Lviv, Lviv Oblast, Ukraine, 79000",
          GoogleTimeZone =
        new GoogleTimeZone() { DstOffset = 0, RawOffset = 7200, Status = "OK", TimeZoneId = "Europe / Kiev", TimeZoneName = "Eastern European Standard Time" }
        });
      AddressRequest testAddress = new AddressRequest
      {
        LocationAddress = "Lviv"
      };

      // Act
      var response = _addressController.Post(testAddress);

      // Assert
      Assert.IsInstanceOf<Location>(response);
      Assert.AreEqual("uk-UA", response.CountryCode);
      Assert.AreEqual("Lviv, Lviv Oblast, Ukraine, 79000", response.FormattedAddress);
      Assert.AreEqual(49.839683, response.Latitude);
      Assert.AreEqual(24.029717, response.Longitude);
      Assert.AreEqual(0, response.GoogleTimeZone.DstOffset);
      Assert.AreEqual(7200, response.GoogleTimeZone.RawOffset);
      Assert.AreEqual("OK", response.GoogleTimeZone.Status);
      Assert.AreEqual("Europe / Kiev", response.GoogleTimeZone.TimeZoneId);
      Assert.AreEqual("Eastern European Standard Time", response.GoogleTimeZone.TimeZoneName);
    }

    [Test]
    public void LocationService_GetTimeZoneJsone()
    {
      // Arrange
      _locationService.Setup(y => y.GetTimeZoneJsone(It.IsAny<Location>(), It.IsAny<TimeSpan>()))
        .ReturnsAsync(new GoogleTimeZone() { DstOffset = 0, RawOffset = 7200, Status = "OK", TimeZoneId = "Europe / Kiev", TimeZoneName = "Eastern European Standard Time" });
      AddressRequest testAddress = new AddressRequest
      {
        LocationAddress = "Lviv"
      };

      // Act
      var response = _locationService.Object.GetTimeZoneJsone(new Location(),new TimeSpan()).Result;

      // Assert
      Assert.IsInstanceOf<GoogleTimeZone>(response);
      Assert.AreEqual(0, response.DstOffset);
      Assert.AreEqual(7200, response.RawOffset);
      Assert.AreEqual("OK", response.Status);
      Assert.AreEqual("Europe / Kiev", response.TimeZoneId);
      Assert.AreEqual("Eastern European Standard Time", response.TimeZoneName);
    }
  }
}