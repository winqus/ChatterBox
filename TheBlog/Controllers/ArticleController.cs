using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBlog.Common.Constants;
using TheBlog.Filters;
using TheBlog.Interfaces;
using TheBlog.Models;
using TheBlog.ViewModels.ArticleViewModels;

namespace TheBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IResourceManager _resourceManager;

        private readonly IArticleData _articleDb;

        public ArticleController(IResourceManager resourceManager, IArticleData articleDb)
        {
            _resourceManager = resourceManager;
            _articleDb = articleDb;
        }

        [HttpGet("GetAll")]
        [ResponseCache(Duration = 30)]
        public IActionResult GetAll()
        {
            var articles = _articleDb.GetArticles();

            var viewArticles = articles.Select(article => new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Text = article.Text,
                Created = article.Created,
                AuthorName = article.ApplicationUser.UserName!,
                Image = article.ImageFile == null ? null : new ImageViewModel
                {
                    Id = article.ImageFile.Id!,
                    Href = Url.Action("Image", "Resource", new { id = article.ImageFile.Id })!,
                    Alt = $"Image for {article.Title}"
                }
            }).OrderByDescending(a => a.Created).ToList();

            return Ok(viewArticles);
        }

        [Authorize]
        [ValidateModel]
        [HttpPost("Create")]
        [UserInRole(UserRoles.ArticleCreator, "User does not have permission to post articles.")]
        public IActionResult Create([FromForm] CreateArticleViewModel model)
        {
            var newArticle = _articleDb.AddArticle(new Article
            {
                ApplicationUserId = User!.Identity.GetSubjectId(),
                Title = model.Title,
                Text = model.Text,
                Created = DateTime.Now,
            });

            if (newArticle == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (model.Image != null)
            {
                var storedImage = _resourceManager.SaveImage(model.Image, ModelState, newArticle.Id);
                if (storedImage == null)
                {
                    return new BadRequestObjectResult(ModelState);
                }
            }

            return Ok(model);
        }

        [Authorize]
        [ValidateModel]
        [HttpPut("Update")]
        [UserInRole(UserRoles.ArticleCreator, "User does not have permission to edit articles.")]
        public IActionResult Update([FromForm] UpdateArticleViewModel model)
        {
            var article = _articleDb.GetArticle(model.Id);

            if (article == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (model.Title != null)
            {
                article.Title = model.Title;
            }

            if (model.Text != null)
            {
                article.Text = model.Text;
            }

            if (model.Image != null)
            {
                var storedImage = _resourceManager.SaveImage(model.Image, ModelState, article.Id);

                if (storedImage == null)
                {
                    return new BadRequestObjectResult(ModelState);
                }
            }

            _articleDb.UpdateArticle(article);

            return Ok(model);
        }

        [Authorize]
        [HttpDelete("Delete")]
        [UserInRole(UserRoles.ArticleCreator, "User does not have permission to delete articles.")]
        public IActionResult Delete([FromQuery] string id)
        {
            var article = _articleDb.GetArticle(id);

            if (article == null)
            {
                ModelState.AddModelError("Id", "Invalid article id.");
                return new BadRequestObjectResult(ModelState);
            }

            _resourceManager.DeleteImage(article.ImageFile);
            _articleDb.DeleteArticle(article);

            return Ok();
        }
    }
}
