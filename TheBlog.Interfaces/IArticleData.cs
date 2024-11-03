using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBlog.Models;

namespace TheBlog.Interfaces
{
    public interface IArticleData
    {
        Article? AddArticle(Article article);

        Article? GetArticle(string id);

        IEnumerable<Article> GetArticles();

        Article? UpdateArticle(Article article);

        void DeleteArticle(Article article);

        ArticleComment? AddComment(ArticleComment comment);

        ArticleComment? GetComment(string commentId);

        List<ArticleComment> GetArticleComments(Article article);

        ArticleComment? UpdateComment(ArticleComment comment);

        void DeleteComment(ArticleComment comment);
    }
}
