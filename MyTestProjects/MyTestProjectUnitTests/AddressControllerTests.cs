using NUnit.Framework;
using Microsoft.Extensions.Logging;
using MyTestProject.Controllers;
using Moq;
using MyTestProject.Services;
using Microsoft.Extensions.Configuration;
using MyTestProject.Models;
using System;

namespace MyTestProjectUnitTests
{
  public class AddressControllerTests
  {
    private Mock<ILogger<AddressController>> _logger;
    private Mock<ILocationService> _locationService;
    private Mock<IConfiguration> _config;
    private AddressController _addressController;
    

    [SetUp]
    public void Setup()
    {
      // Arrange
      _logger = new Mock<ILogger<AddressController>>();
      _config = new Mock<IConfiguration>();
      _locationService = new Mock<ILocationService>();
      _locationService.Setup(x => x.GetGeocodeJsone(It.IsAny<string>())).ReturnsAsync(new Location() { CountryCode = "UA", Latitude = 0, Longitude = 0, FormattedAddress = "" });
      _locationService.Setup(y => y.GetTimeZoneJsone(It.IsAny<Location>(), It.IsAny<TimeSpan>())).ReturnsAsync(new GoogleTimeZone() {});
      _addressController = new AddressController(_logger.Object, _config.Object, _locationService.Object);
    }

    [Test]
    public void Address_Controller_ShouldReturnLocationResult()
    {
      // Arrange
      AddressRequest testAddress = new AddressRequest
      {
        locationAddress = "Lviv"
      };

      // Act
      var response = _addressController.Post(testAddress);

      // Assert
      Assert.IsInstanceOf<Location>(response);
      Assert.Pass();
    }

    [Test]
    public void Address_Controller_ShouldReturnLocationResultWithData()
    {
      // Arrange
      _locationService.Setup(x => x.GetGeocodeJsone(It.IsAny<string>()))
        .ReturnsAsync(new Location() { CountryCode = "UA", Latitude = 49.839683, Longitude = 24.029717, FormattedAddress = "Lviv, Lviv Oblast, Ukraine, 79000", googleTimeZone = null });
      _locationService.Setup(y => y.GetTimeZoneJsone(It.IsAny<Location>(), It.IsAny<TimeSpan>()))
        .ReturnsAsync(new GoogleTimeZone() { dstOffset = 0, rawOffset = 7200, status = "OK", timeZoneId = "Europe / Kiev", timeZoneName = "Eastern European Standard Time" });
      AddressRequest testAddress = new AddressRequest
      {
        locationAddress = "Lviv"
      };

      // Act
      var response = _addressController.Post(testAddress);

      // Assert
      Assert.IsInstanceOf<Location>(response);
      Assert.AreEqual("uk-UA", response.CountryCode);
      Assert.AreEqual("Lviv, Lviv Oblast, Ukraine, 79000", response.FormattedAddress);
      Assert.AreEqual(49.839683, response.Latitude);
      Assert.AreEqual(24.029717, response.Longitude);
      Assert.AreEqual( 0, response.googleTimeZone.dstOffset);
      Assert.AreEqual(7200,response.googleTimeZone.rawOffset);
      Assert.AreEqual( "OK",response.googleTimeZone.status);
      Assert.AreEqual("Europe / Kiev", response.googleTimeZone.timeZoneId);
      Assert.AreEqual("Eastern European Standard Time", response.googleTimeZone.timeZoneName);
    }
  }
}