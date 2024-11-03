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
    [Route("api/article/{articleId}/[controller]")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IArticleData _articleDb;

        public CommentController(IArticleData articleDb, UserManager<ApplicationUser> userManager)
        {
            _articleDb = articleDb;
            _userManager = userManager;
        }

        [HttpGet("All")]
        [AllowAnonymous]
        public IActionResult GetComments([FromRoute] string articleId)
        {
            var article = _articleDb.GetArticle(articleId);

            if (article == null)
            {
                ModelState.AddModelError("ArticleId", "So article with this id exists.");
                return BadRequest(ModelState);
            }

            var comments = _articleDb.GetArticleComments(article)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Author = c.User.UserName ?? "N/A",
                    Created = c.Created,
                    Text = c.Text,
                })
                .OrderBy(c => c.Created)
                .ToList();
            return Ok(comments);
        }

        [HttpPost]
        [ValidateModel]
        [UserInRole(UserRoles.Commentator, "User does not have permission to post comments.")]
        public IActionResult PostComment([FromForm] CreateCommentViewModel model)
        {
            var user = _userManager.FindByIdAsync(User!.Identity.GetSubjectId()).Result;

            var article = _articleDb.GetArticle(model.ArticleId);

            if (article == null)
            {
                ModelState.AddModelError("Error", "Article does not exist.");
                return BadRequest(ModelState);
            }

            var newComment = _articleDb.AddComment(new ArticleComment
            {
                Text = model.Text,
                Created = DateTime.Now,
                Article = article,
                User = user!,
            });

            if (newComment == null)
            {
                ModelState.AddModelError("Error", "Failed to create comment.");
                return BadRequest(ModelState);
            }

            return Ok(model);
        }

        [HttpPatch]
        [ValidateModel]
        [UserInRole(UserRoles.Commentator, "User does not have permission to update comments.")]
        public IActionResult UpdateComment([FromForm] UpdateCommentViewModel model)
        {
            var comment = _articleDb.GetComment(model.CommentId);

            if (comment == null)
            {
                ModelState.AddModelError("Error", "Comment does not exist.");
                return BadRequest(ModelState);
            }

            comment.Text = model.Text;

            var updatedComment = _articleDb.UpdateComment(comment);

            if (updatedComment == null)
            {
                ModelState.AddModelError("Error", "Failed to update comment.");
                return BadRequest(ModelState);
            }

            return Ok(model);
        }

        [HttpDelete]
        [UserInRole(UserRoles.Commentator, "User does not have permission to delete comments.")]
        public IActionResult DeleteComment([FromQuery] string commentId)
        {
            var comment = _articleDb.GetComment(commentId);

            if (comment == null)
            {
                ModelState.AddModelError("Error", "Comment does not exist.");
                return BadRequest(ModelState);
            }

            _articleDb.DeleteComment(comment);

            return Ok();
        }
    }
}
