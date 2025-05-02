
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Controllers;
using SE.Neo.WebAPI.Models.Article;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Security.Claims;

namespace SE.Neo.UnitTests.Controllers
{
    [TestFixture]
    public class ArticleControllerTests
    {
        private readonly Mock<IArticleApiService> _mockArticleAPI;
        private readonly Mock<ILogger<ArticleController>> _mocklogger;
        private readonly ArticleController _mockController;
        private const string FilterForYou = "foryou";

        public ArticleControllerTests()
        {
            _mocklogger = new Mock<ILogger<ArticleController>>();
            _mockArticleAPI = new Mock<IArticleApiService>();
            _mockController = new ArticleController(_mockArticleAPI.Object, _mocklogger.Object);

        }

        [SetUp]
        public void Setup()
        {
            _mockController.ControllerContext = new ControllerContext();
            _mockController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]

            {
                new Claim(ClaimTypes.Name, "Oleh Admin"),
                new Claim(ClaimTypes.GivenName, "Oleh"),
                new Claim(ClaimTypes.Surname, "Admin"),
                new Claim(ClaimTypes.Email, "o.kachmar@abtollc.com"),
                new Claim(ClaimTypes.Role, "Admin")

            }));
            _mockController.ControllerContext.HttpContext.Items = new Dictionary<object, object?>();
            _mockController.ControllerContext.HttpContext.Items.Add("User", new UserModel
            {
                FirstName = "Oleh",
                LastName = "Admin",
                Username = "Oleh Admin",
                Id = 1,
                RoleIds = new List<int>() { 1, 5 }
            });
        }

        [Test]
        public async Task getArticles_emptyValues_Test()
        {
            BaseSearchFilterModel? model = new()
            {
                Expand = null,
                FilterBy = null,
                OrderBy = null,
                Random = null,
                IncludeCount = false,
                Search = null,
                Take = null,
                Skip = null
            };
            IActionResult result = await _mockController.GetArticles(model);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task getArticles_Null_Test()
        {
            BaseSearchFilterModel? model = null;
            IActionResult result = await _mockController.GetArticles(model);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task getArticles_Test_Valid_Values()
        {
            BaseSearchFilterModel? model = new()
            {
                Expand = "categories",
                FilterBy = FilterForYou,
                OrderBy = "date.asc",
                Random = null,
                IncludeCount = true,
                Search = null,
                Take = 30,
                Skip = 0
            };
            IActionResult result = await _mockController.GetArticles(model);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        [TestCase(1019, null)]
        [TestCase(1019, "categories,regions,solutions,technologies")]
        public async Task getArticle_By_Valid_Id(int id, string? expand)
        {
            var currentUser = (UserModel)_mockController.ControllerContext.HttpContext.Items["User"]!;
            _mockArticleAPI.Setup(service => service.GetArticleAsync(id, currentUser, expand)).ReturnsAsync(getMockArticle());
            IActionResult result = await _mockController.GetArticle(id, expand);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task getArticle_By_Value_Zero()
        {
            IActionResult result = await _mockController.GetArticle(0, null);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task SyncContents()
        {
            IActionResult result = await _mockController.SyncContents();
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task PostArticle_By_InValid_Id()
        {
            int id = 1;
            IActionResult result = await _mockController.PostArticleTrending(id);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task PostArticle_By_Valid_Id()
        {
            int id = 1019;
            IActionResult result = await _mockController.PostArticleTrending(id);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetArticleTrendings_With_Valid_Values()
        {
            PaginationModel filter = new()
            {
                Skip = 0,
                Take = 25,
                IncludeCount = true
            };
            IActionResult result = await _mockController.GetArticleTrendings(filter);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetArticleTrendings_With_Null_Values()
        {
            PaginationModel filter = new()
            {
                Skip = null,
                Take = null,
                IncludeCount = false
            };
            IActionResult result = await _mockController.GetArticleTrendings(filter);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        private ArticleResponse getMockArticle()
        {
            return new ArticleResponse
            {
                Id = 1019,
                Date = DateTime.Now,
                Slug = "neo-test-article",
                Modified = DateTime.Now,
                Title = "Neo Test Article",
                Content = "",
                ImageUrl = "",
                VideoUrl = "",
                PdfUrl = "",
                TypeId = 17,
                IsSaved = false,
                Categories = null
            };

        }
    }
}