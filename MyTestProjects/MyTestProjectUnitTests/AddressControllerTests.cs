using NUnit.Framework;
using Microsoft.Extensions.Logging;
using MyTestProject.Controllers;
using Moq;
using MyTestProject.Services;
using Microsoft.Extensions.Configuration;
using MyTestProject.DTO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyTestProjectUnitTests
{
  public class AddressControllerTests
  {
    private Mock<ILogger<AddressController>> _logger;
    private ILocationService _locationService;
    private Mock<IHttpHandler> _client;
    private Mock<IConfiguration> _config;
    private Mock<IConfigurationSection> _mockConfSection;

    [SetUp]
    public void Setup()
    {
      _logger = new Mock<ILogger<AddressController>>();
      _mockConfSection = new Mock<IConfigurationSection>();
      _mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "googleUrl")]).Returns("testurl");
      _mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "key")]).Returns("testKey");
      _config = new Mock<IConfiguration>();
      _config.Setup(a => a.GetSection(It.Is<string>(s => s == "GoogleApiUrl"))).Returns(_mockConfSection.Object);
      _client = new Mock<IHttpHandler>();
      _client.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());
      _locationService = new LocationService(_logger.Object, _config.Object, _client.Object);
    }

    [Test]
    public void Location_Service_GetGeocodeJsone_Should()
    {
      // Arrange
      var testString = "Lviv";
      // Act
      var response = _locationService.GetGeocodeJsone(testString);

      // Assert
      Assert.IsInstanceOf<Task<Location>>(response);
      Assert.Pass();
    }
  }
}