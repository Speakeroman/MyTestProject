using NUnit.Framework;
using Microsoft.Extensions.Logging;
using MyTestProject.Controllers;
using Moq;
using MyTestProject.Services;
using Microsoft.Extensions.Configuration;
using MyTestProject.Models;

namespace MyTestProjectUnitTests
{
  public class Tests
  {
    private Mock<ILogger<AddressController>> _logger;
    private Mock<IHttpHandler> _httpHandler;
    private Mock<IConfiguration> _config;

    private AddressController CreateController() => new AddressController(_logger.Object, _config.Object);

    [SetUp]
    public void Setup()
    {
      _logger = new Mock<ILogger<AddressController>>();
      _config = new Mock<IConfiguration>();
    }

    [Test]
    public void Address_Controller_ShouldReturnLocationResult()
    {
      var controller = CreateController();
      AddressRequest testAddress = new AddressRequest();
      testAddress.locationAddress = "Lviv";
      var response = controller.Post(testAddress);
      Assert.IsInstanceOf<Location>(response);
      Assert.Pass();
    }
  }
}