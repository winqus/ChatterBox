using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlog.Models
{
    public class ArticleComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        [ForeignKey("Article")]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
