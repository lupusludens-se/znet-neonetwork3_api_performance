using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SE.Neo.Common.Models.Activity;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Activity;
using SE.Neo.WebAPI.Services;

namespace SE.Neo.UnitTests.Controllers
{
    public class ActivityApiServiceTests
    {
        private readonly Mock<ILogger<ActivityApiService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ActivityApiService _mockActivityApiService;
        private readonly Mock<IActivityService> _mockActivityService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;


        public ActivityApiServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockActivityService = new Mock<IActivityService>();
            _mockLogger = new Mock<ILogger<ActivityApiService>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _mockActivityApiService = new ActivityApiService(_mockLogger.Object, _mockMapper.Object,
                _mockActivityService.Object, _mockHttpContextAccessor.Object);
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task AddActivity_DefaultModel_Test()
        {
            // Arrange
            var model = new ActivityRequest() { LocationId = ActivityLocation.AccountSettings };
            int expectedResult = 100;
            _mockMapper.Setup(m => m.Map<ActivityDTO>(model)).Returns(new ActivityDTO());
            _mockActivityService.Setup(y => y.CreateActivityAsync(It.IsAny<ActivityDTO>())).ReturnsAsync(expectedResult);
            _ = _mockHttpContextAccessor.SetupGet(x => x.HttpContext.Items).Returns(new Dictionary<object, object> { { "SessionId", Guid.NewGuid() } });


            // Act
            var result = await _mockActivityApiService.CreateActivityAsync(model);

            // Assert
            Assert.IsTrue(result >= 0);

        }



        [Test]
        public async Task AddActivity_NullModel_Test()
        {
            // Arrange
            ActivityRequest model = null;
            int expectedResult = 0;
            _mockMapper.Setup(m => m.Map<ActivityDTO>(model)).Returns(new ActivityDTO());
            _mockActivityService.Setup(y => y.CreateActivityAsync(It.IsAny<ActivityDTO>())).ReturnsAsync(expectedResult);
            _ = _mockHttpContextAccessor.SetupGet(x => x.HttpContext.Items).Returns(new Dictionary<object, object> { { "SessionId", Guid.NewGuid() } });


            // Act
            var result = await _mockActivityApiService.CreateActivityAsync(model);

            // Assert
            Assert.IsTrue(result == 0);

        }
    }

}