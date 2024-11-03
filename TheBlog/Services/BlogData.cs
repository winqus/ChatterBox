using Microsoft.EntityFrameworkCore;
using TheBlog.Data;
using TheBlog.Interfaces;
using TheBlog.Models;

namespace TheBlog.Services
{
    public class BlogData : IArticleData, IFileData
    {
        private readonly ApplicationDbContext _context;

        public BlogData(ApplicationDbContext context)
        {
            _context = context;
        }

        public Article? AddArticle(Article article)
        {
            var newArticle = _context.Articles.Add(article).Entity;
            _context.SaveChanges();

            return newArticle;
        }

        public Article? GetArticle(string id)
        {
            return _context.Articles
                .Include(a => a.ApplicationUser)
                .Include(a => a.ImageFile)
                .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Article> GetArticles()
        {
            return _context.Articles
                .Include(a => a.ApplicationUser)
                .Include(a => a.ImageFile)
                .ToList();
        }

        public Article? UpdateArticle(Article article)
        {
            var updatedArticle = _context.Update(article).Entity;
            _context.SaveChanges();
            return updatedArticle;
        }

        public void DeleteArticle(Article article)
        {
            _context.Remove(article);
            _context.SaveChanges();
        }

        public ImageFile? AddImage(ImageFile image)
        {
            var newImage = _context.Images.Add(image).Entity;
            _context.SaveChanges();

            return newImage;
        }

        public ImageFile? GetImage(string id)
        {
            return _context.Images.FirstOrDefault(image => image.Id == id);
        }

        public IEnumerable<ImageFile> GetImages()
        {
            return _context.Images.ToList();
        }

        public void RemoveImage(ImageFile image)
        {
            _context.Remove(image);
            _context.SaveChanges();
        }

        public ArticleComment? AddComment(ArticleComment comment)
        {
            var newComment = _context.ArticleComments.Add(comment).Entity;
            _context.SaveChanges();

            return newComment;
        }

        public ArticleComment? GetComment(string commentId)
        {
            var comment = _context.ArticleComments
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == commentId);
            return comment;
        }

        List<ArticleComment> IArticleData.GetArticleComments(Article article)
        {
            var comments = _context.ArticleComments
                .Include(a => a.User)
                .Where(a => a.ArticleId == article.Id).ToList();
            return comments;
        }

        public ArticleComment? UpdateComment(ArticleComment comment)
        {
            var updatedComment = _context.ArticleComments.Update(comment).Entity;
            _context.SaveChanges();

            return updatedComment;
        }

        public void DeleteComment(ArticleComment comment)
        {
            _context.ArticleComments.Remove(comment);
            _context.SaveChanges();
        }
    }
}
