using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System.Data;
using System.Security.Claims;
using System.Text;
using TheBlog.Controllers;
using TheBlog.Interfaces;
using TheBlog.Models;
using TheBlog.ViewModels.ArticleViewModels;

namespace TheBlog.Tests
{
    public class ArticleControllerTests
    {
        [Fact]
        public void GetAll_ReturnsCorrectArticleViewData()
        {
            // Arrange
            var expectedArticles = new List<Article>
            {
                new Article
                {
                    Id = "article-1",
                    ApplicationUser = new ApplicationUser { UserName = "TestUser1" },
                    Created = DateTime.Now,
                    Title = "Title 1",
                    Text = "Text 1",
                },
                new Article
                {
                    Id = "article-2",
                    ApplicationUser = new ApplicationUser { UserName = "TestUser2" },
                    Created = DateTime.Now,
                    Title = "Title 2",
                    Text = "Text 2",
                },
            }.OrderByDescending(a => a.Created).ToList();

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(x => x.GetArticles()).Returns(expectedArticles);

            var dummyResourceManager = new Mock<IResourceManager>();

            var controller = new ArticleController(dummyResourceManager.Object, mockArticleDb.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var viewArticles = viewResult.Value as List<ArticleViewModel>;

            Assert.NotNull(viewArticles);
            Assert.Equal(expectedArticles.Count, viewArticles.Count);

            for (int i = 0; i < expectedArticles.Count; i++)
            {
                var expected = expectedArticles[i];
                var actual = viewArticles[i];

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Title, actual.Title);
                Assert.Equal(expected.Text, actual.Text);
                Assert.Equal(expected.Created, actual.Created);
                Assert.Equal(expected.ApplicationUser.UserName, actual.AuthorName);
            }
        }

        [Fact]
        public void GetAll_ReturnsEmptyList_WhenNoArticlesExist()
        {
            // Arrange
            var expectedArticles = new List<Article>();

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(x => x.GetArticles()).Returns(expectedArticles);

            var dummyResourceManager = new Mock<IResourceManager>();

            var controller = new ArticleController(dummyResourceManager.Object, mockArticleDb.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var viewArticles = viewResult.Value as List<ArticleViewModel>;

            Assert.NotNull(viewArticles);
            Assert.Empty(viewArticles);
        }

        [Fact]
        public void Create_AddsNewArticleToDb_IfModelAndImageAreValid()
        {
            // Arrange
            var model = new CreateArticleViewModel
            {
                Title = "Test Article Title",
                Text = "This is some test text.",
                Image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("A fake image.")), 0, 0, "testImageName", "testImage.jpg")
            };

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(x => x.AddArticle(It.IsAny<Article>())).Returns(new Article { Id = "some-id", Title = model.Title, Text = model.Text, Created = DateTime.Now });

            var mockResourceManager = new Mock<IResourceManager>();
            mockResourceManager.Setup(x => x.SaveImage(It.IsAny<IFormFile>(), It.IsAny<ModelStateDictionary>(), It.IsAny<string>())).Returns(new ImageFile());

            var controller = new ArticleController(mockResourceManager.Object, mockArticleDb.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "id-1"),
            }, "mock"));

            // Act
            var result = controller.Create(model);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = viewResult.Value as CreateArticleViewModel;

            Assert.NotNull(viewModel);
            mockArticleDb.Verify(x => x.AddArticle(It.IsAny<Article>()), Times.Once());
            mockResourceManager.Verify(x => x.SaveImage(It.IsAny<IFormFile>(), It.IsAny<ModelStateDictionary>(), It.IsAny<string>()), Times.Once());
            Assert.Equal(model.Title, viewModel.Title);
            Assert.Equal(model.Text, viewModel.Text);
        }

        [Fact]
        public void Update_UpdatesAnArticleInDb_IfModelAndImageAreValid()
        {
            // Arrange
            var model = new UpdateArticleViewModel
            {
                Id = "article-id-1",
                Title = "Updated test Article Title",
                Text = "Updated test text.",
                Image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Updated fake image.")), 0, 0, "updatedTestImageName", "updatedTestImage.jpg")
            };

            var articleFromDb = new Article { Id = model.Id, Title = model.Title, Text = model.Text, Created = DateTime.Now };

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(x => x.GetArticle(model.Id)).Returns(articleFromDb);

            var mockResourceManager = new Mock<IResourceManager>();
            mockResourceManager.Setup(x => x.SaveImage(It.IsAny<IFormFile>(), It.IsAny<ModelStateDictionary>(), It.IsAny<string>())).Returns(new ImageFile());

            var controller = new ArticleController(mockResourceManager.Object, mockArticleDb.Object);

            // Act
            var result = controller.Update(model);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = viewResult.Value as UpdateArticleViewModel;

            Assert.NotNull(viewModel);
            mockArticleDb.Verify(x => x.GetArticle(model.Id), Times.Once());
            mockArticleDb.Verify(x => x.UpdateArticle(It.IsAny<Article>()), Times.Once());
            mockResourceManager.Verify(x => x.SaveImage(It.IsAny<IFormFile>(), It.IsAny<ModelStateDictionary>(), It.IsAny<string>()), Times.Once());
            Assert.Equal(model.Title, viewModel.Title);
            Assert.Equal(model.Text, viewModel.Text);
        }

        [Fact]
        public void Delete_RemovesAnArticleFromDb()
        {
            // Arrange
            var articleInDb = new Article
            {
                Id = "user-id-1",
                Title = "Title 1",
                Text = "Text 1",
                Created = DateTime.Now
            };

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(x => x.GetArticle(articleInDb.Id)).Returns(articleInDb);

            var mockResourceManager = new Mock<IResourceManager>();

            var controller = new ArticleController(mockResourceManager.Object, mockArticleDb.Object);

            // Act
            var result = controller.Delete(articleInDb.Id);

            // Assert
            var viewResult = Assert.IsType<OkResult>(result);

            Assert.NotNull(viewResult);
            mockArticleDb.Verify(x => x.GetArticle(articleInDb.Id), Times.Once());
            mockArticleDb.Verify(x => x.DeleteArticle(It.IsAny<Article>()), Times.Once());
            mockResourceManager.Verify(x => x.DeleteImage(It.IsAny<ImageFile>()), Times.Once());
        }
    }
}