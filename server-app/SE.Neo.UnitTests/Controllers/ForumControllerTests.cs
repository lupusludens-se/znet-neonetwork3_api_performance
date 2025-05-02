using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Controllers;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Forum;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.UnitTests.Controllers
{
    internal class ForumControllerTests
    {
        private readonly ForumController _mockForumController;
        private readonly Mock<IForumApiService> _mockForumApiService;
        private readonly Mock<ILogger<ForumController>> _mockLogger;
        private readonly Mock<HttpContext> _httpContextMock;
        public ForumControllerTests()
        {
            _mockLogger = new Mock<ILogger<ForumController>>();
            _mockForumApiService = new Mock<IForumApiService>();
            _mockForumController = new ForumController(_mockForumApiService.Object, _mockLogger.Object);
            _httpContextMock = new Mock<HttpContext>();
        }

        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public async Task CreateForumAsync_DefaultModel_Test()
        {
            //Arrange
            var currentUser = new UserModel()
            {
                PermissionIds = new List<int>
                                { (int) PermissionType.ForumManagement }
            };
            _ = _httpContextMock.SetupGet(x => x.Items).Returns(value: new Dictionary<object, object> { { "User", currentUser } });
            _mockForumController.ControllerContext.HttpContext = _httpContextMock.Object;

            ForumRequest model = GetForumModel();

            //Act
            IActionResult result = await _mockForumController.CreateForumAsync(model);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateForumAsync__WithoutPermission_PrivateForum_Test()
        {
            //Arrange
            var currentUser = new UserModel()
            {
                PermissionIds = new List<int>
                                { (int) PermissionType.ToolManagement }
            };
            _ = _httpContextMock.SetupGet(x => x.Items).Returns(value: new Dictionary<object, object> { { "User", currentUser } });
            _mockForumController.ControllerContext.HttpContext = _httpContextMock.Object;

            ForumRequest model = GetForumModel();
            model.IsPrivate = true;


            //Act
            IActionResult result = await _mockForumController.CreateForumAsync(model);

            //Assert
            Assert.IsInstanceOf<ForbidResult>(result);
        }

        [Test]
        public async Task CreateForumAsync_WithoutPermission_PinnedForum_Test()
        {
            //Arrange
            var currentUser = new UserModel()
            {
                PermissionIds = new List<int>
                                { (int) PermissionType.MessagesAll }
            };
            _ = _httpContextMock.SetupGet(x => x.Items).Returns(value: new Dictionary<object, object> { { "User", currentUser } });
            _mockForumController.ControllerContext.HttpContext = _httpContextMock.Object;

            ForumRequest model = GetForumModel();
            model.IsPinned = true;


            //Act
            IActionResult result = await _mockForumController.CreateForumAsync(model);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private ForumRequest GetForumModel()
        {
            return new ForumRequest()
            {
                Subject = "Delhi",
                IsPinned = false,
                IsPrivate = false,
                FirstMessage = new ForumFirstMessageRequest()
                {
                    Text = "Delhi Discussion",
                    TextContent = "Issue",
                    Id = 0,
                    Attachments = new List<MessageAttachmentRequest>() { }
                },
                Users = new List<ForumUserRequest>() { },
                Regions = new List<RegionRequest>() { },
                Categories = new List<CategoryRequest>() {
                   new CategoryRequest(){Id = 43},
                   new CategoryRequest(){ Id = 48}
           }
            };
        }
    }
}
