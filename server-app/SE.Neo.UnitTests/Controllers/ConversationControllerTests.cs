using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Controllers;
using SE.Neo.WebAPI.Models.Conversation;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.UnitTests.Controllers
{
    public class ConversationControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IConversationApiService> _mockConversationApiService;
        private readonly Mock<ILogger<ConversationController>> _mockLogger;
        private readonly ConversationController _controller;
        private readonly Mock<HttpContext> _httpContextMock;


        public ConversationControllerTests()
        {
            _mockConversationApiService = new Mock<IConversationApiService>();
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<ConversationController>>();
            _controller = new ConversationController(_mockConversationApiService.Object, _mockLogger.Object, _mockUserService.Object);
            _httpContextMock = new Mock<HttpContext>();

        }

        [SetUp]
        public void Setup()
        {
        }

        #region Testcases - GetConversationsAsync

        [Test]
        public async Task GetConversationsAsync_DefaultEmptyModel_Test()
        {
            // Arrange
            var currentUser = new UserModel() { PermissionIds = new List<int> { (int)PermissionType.MessagesAll } };
            _ = _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            ConversationsFilter defaultFilter = new();
            _mockConversationApiService.Setup(x => x.GetConversationsAsync(It.IsAny<UserModel>(), It.IsAny<ConversationsFilter>()))
                .ReturnsAsync(new WrapperModel<ConversationResponse>() { Count = 100 });

            // Act 
            var result = await _controller.GetConversationsAsync(defaultFilter);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetConversationsAsync_NullModel_Test()
        {
            // Arrange
            var currentUser = new UserModel() { PermissionIds = new List<int> { (int)PermissionType.MessagesAll } };
            _ = _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            ConversationsFilter nullFilter = null;
            _mockConversationApiService.Setup(x => x.GetConversationsAsync(It.IsAny<UserModel>(), It.IsAny<ConversationsFilter>()))
                .ReturnsAsync(new WrapperModel<ConversationResponse>() { Count = 0 });

            // Act 
            var result = await _controller.GetConversationsAsync(nullFilter);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetConversationsAsync_DefaultModel_Test()
        {
            // Arrange
            var currentUser = new UserModel() { PermissionIds = new List<int> { (int)PermissionType.MessagesAll } };
            _ = _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            ConversationsFilter validFilter = new ConversationsFilter()
            {
                IncludeAll = true,
                Individual = true,
                IncludeCount = true,
                Take = 25,
                Skip = 0
            };

            _mockConversationApiService.Setup(x => x.GetConversationsAsync(It.IsAny<UserModel>(), It.IsAny<ConversationsFilter>()))
                .ReturnsAsync(new WrapperModel<ConversationResponse>() { Count = 100 });

            // Act 
            var result = await _controller.GetConversationsAsync(validFilter);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion

        #region Testcases - GetConversationsAsync
        [Test]
        public async Task GetConversationsAsync_DefaultModel_ReturnsOkResult()
        {
            // Arrange
            int id = 194;
            string expand = null;
            UserModel currentUser = new UserModel();
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            ConversationResponse item = new ConversationResponse() { Id = 104 };
            _mockConversationApiService.Setup(x => x.GetConversationAsync(currentUser, id, expand)).ReturnsAsync(item);

            // Act
            var result = await _controller.GetConversationsAsync(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetConversationsAsync_DefaultModel_ReturnsNotFoundResult()
        {
            // Arrange
            int id = 1;
            string expand = "test";
            UserModel currentUser = new UserModel();
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            //ConversationResponse item = new ConversationResponse() { Id = 200 };
            _mockConversationApiService.Setup(x => x.GetConversationAsync(currentUser, id, expand)).ReturnsAsync((ConversationResponse?)null);

            // Act
            var result = await _controller.GetConversationsAsync(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        #endregion

        #region
        [Test]
        public async Task CreateConversationsMessageAsync_DefaultModel_ReturnsOkResult()
        {
            // Arrange
            int id = 100;
            UserModel currentUser = new UserModel() { Id = 101 };
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            var expectedResponse = new ConversationMessageResponse() { Id = 104 };
            ConversationMessageRequest model = new ConversationMessageRequest() { Text = "abc" };
            _mockConversationApiService.Setup(x => x.CreateConversationMessageAsync(currentUser, id, model)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateConversationMessageAsync(id, model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateConversationsMessageAsync_NullModel_ReturnsOkResult()
        {
            // Arrange
            int id = 100;
            UserModel currentUser = new UserModel() { Id = 101 };
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;
            var expectedResponse = new ConversationMessageResponse() { Id = 104 };
            ConversationMessageRequest model = null;
            _mockConversationApiService.Setup(x => x.CreateConversationMessageAsync(currentUser, id, model)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateConversationMessageAsync(id, model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion

        #region

        [Test]
        public async Task MarkUserMessagesAsReadAsync()
        {
            int id = 1000;
            UserModel currentUser = new UserModel() { Id = 101 };
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;


            var expectedResponse = new ConversationMessageResponse() { Id = 104 };
            ConversationMessageRequest model = null;
            _mockConversationApiService.Setup(x => x.MarkUserMessagesAsReadAsync(It.IsAny<UserModel>(), It.IsAny<int>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _controller.MarkUserMessagesAsReadAsync(id);

            // Assert
            _mockConversationApiService.Verify();

        }




        #endregion

        #region CreateContactUsRequest
        [Test]
        public async Task CreateContactUsRequest_DefaultModel_ReturnsOkResult()
        {
            // Arrange
            UserModel currentUser = new UserModel() { Id = 101 };
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;

            var expectedResponse = new ConversationResponse() { Id = 104 };
            ConversationContactUsRequest model = new ConversationContactUsRequest() { Message = "message", Subject = "subject" };
            _mockConversationApiService.Setup(x => x.CreateContactUsConversationAsync(currentUser, model)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateContactUsConversationAsync(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateContactUsRequest_NullModel_ReturnsOkResult()
        {
            // Arrange
            UserModel currentUser = new UserModel() { Id = 101 };
            _httpContextMock.SetupGet(x => x.Items).Returns(new Dictionary<object, object> { { "User", currentUser } });
            _controller.ControllerContext.HttpContext = _httpContextMock.Object;

            var expectedResponse = new ConversationResponse() { Id = 104 };
            ConversationContactUsRequest model = null;
            _mockConversationApiService.Setup(x => x.CreateContactUsConversationAsync(currentUser, model)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateContactUsConversationAsync(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion


    }
}
