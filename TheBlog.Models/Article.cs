using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlog.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string Title { get; set; }

        public string? Text { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ImageFile ImageFile { get; set; }

        public virtual ICollection<ArticleComment> ArticleComments { get; set; }
    }
}
