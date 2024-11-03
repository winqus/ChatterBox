using Microsoft.AspNetCore.Identity;

namespace TheBlog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<ArticleComment> ArticleComments { get; set; }
    }
}