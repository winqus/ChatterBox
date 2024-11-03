using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System.Security.Claims;
using System.Text;
using TheBlog.Common.Constants;
using TheBlog.Controllers;
using TheBlog.Interfaces;
using TheBlog.Models;
using TheBlog.Tests.MockObjects;
using TheBlog.ViewModels.ArticleViewModels;

namespace TheBlog.Tests
{
    public class CommentControllerTests
    {
        [Fact]
        public void GetComments_ReturnsOkAndListOfComments()
        {
            // Arrange
            var article = new Article { Id = "article-id", Title = "Title 1" };
            var expectedComments = new List<ArticleComment>
            {
                new ArticleComment { Id = "comment-1", Text = "This is the first comment.", Created = DateTime.Now, User = new ApplicationUser { UserName = "User1" } },
                new ArticleComment { Id = "comment-2", Text = "This is the second comment.",  Created = DateTime.Now, User = new ApplicationUser { UserName = "User2" } }
            }.OrderBy(c => c.Created).ToList();

            var dummyUserManager = new FakeUserManager();

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(x => x.GetArticle(article.Id)).Returns(article);
            mockArticleDb.Setup(x => x.GetArticleComments(article)).Returns(expectedComments);

            var controller = new CommentController(mockArticleDb.Object, dummyUserManager);

            // Act
            var result = controller.GetComments(article.Id);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var commentsResult = Assert.IsType<List<CommentViewModel>>(viewResult.Value);
            Assert.Equal(expectedComments.Count, commentsResult.Count);
            for (int i = 0; i < expectedComments.Count; i++)
            {
                Assert.Equal(expectedComments[i].Text, commentsResult[i].Text);
                Assert.Equal(expectedComments[i].User.UserName, commentsResult[i].Author);
                Assert.Equal(expectedComments[i].Created, commentsResult[i].Created);
            }
        }

        [Fact]
        public void PostComment_AddsNewCommentToDb_IfModelIsValid()
        {
            // Arrange
            var article = new Article { Id = "article-id", Title = "Test article" };
            var user = new ApplicationUser { Id = "user-id", UserName = "TestUser" };
            var model = new CreateCommentViewModel
            {
                ArticleId = article.Id,
                Text = "This is some test text.",
            };
            var expectedComment = new ArticleComment
            {
                Id = "comment-id",
                Text = model.Text,
                Created = DateTime.Now,
                Article = article,
                User = user,
            };

            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            var mockArticleDb = new Mock<IArticleData>();
            mockArticleDb.Setup(a => a.GetArticle(It.IsAny<string>())).Returns(article);
            mockArticleDb.Setup(a => a.AddComment(It.IsAny<ArticleComment>())).Returns(expectedComment);

            var controller = new CommentController(mockArticleDb.Object, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "id-1"),
            }, "mock"));

            // Act
            var result = controller.PostComment(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = Assert.IsType<CreateCommentViewModel>(okResult.Value);
            Assert.Equal(model.Text, viewModel.Text);
        }

    }
}
