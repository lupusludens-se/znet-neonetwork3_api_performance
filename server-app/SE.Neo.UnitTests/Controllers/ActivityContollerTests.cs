using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Controllers;
using SE.Neo.WebAPI.Models.Activity;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.UnitTests.Controllers
{
    public class ActivityContollerTests
    {
        private readonly Mock<IActivityApiService> _mockService;
        private readonly Mock<ILogger<ActivityContoller>> _mockLogger;
        private readonly ActivityContoller _controller;

        public ActivityContollerTests()
        {
            _mockService = new Mock<IActivityApiService>();
            _mockLogger = new Mock<ILogger<ActivityContoller>>();
            _controller = new ActivityContoller(_mockService.Object, _mockLogger.Object);
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task AddActivity_NullModel_Test()
        {
            // Arrange
            int expectedActivityId = 0;
            _mockService.Setup(x => x.CreateActivityAsync(It.IsAny<ActivityRequest>())).ReturnsAsync(expectedActivityId);
            ActivityRequest model = null;

            // Act
            var result = await _controller.AddActivity(model);

            // Assert
            //Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddActivity_ValidModel_Test()
        {
            // Arrange
            int expectedActivityId = 100;
            _mockService.Setup(x => x.CreateActivityAsync(It.IsAny<ActivityRequest>())).ReturnsAsync(expectedActivityId);
            ActivityRequest model = new ActivityRequest()
            {
                LocationId = ActivityLocation.AccountSettings,
                TypeId = Core.Enums.ActivityType.CompanyFollow,
                Details = "Test"
            };

            // Act
            var result = await _controller.AddActivity(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddActivity_EmptyModel_Test()
        {
            // Arrange
            int expectedActivityId = 0;
            _mockService.Setup(x => x.CreateActivityAsync(It.IsAny<ActivityRequest>())).ReturnsAsync(expectedActivityId);
            ActivityRequest model = new();

            // Act
            var result = await _controller.AddActivity(model);

            // Assert
            //Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddActivity_EmptyDetails_Test()
        {
            // Arrange
            int expectedActivityId = 100;
            _mockService.Setup(x => x.CreateActivityAsync(It.IsAny<ActivityRequest>())).ReturnsAsync(expectedActivityId);
            ActivityRequest model = new ActivityRequest()
            {
                LocationId = ActivityLocation.AccountSettings,
                TypeId = Core.Enums.ActivityType.CompanyFollow,
                Details = null
            };

            // Act
            var result = await _controller.AddActivity(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

    }
}