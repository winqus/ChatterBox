using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TheBlog.Common.Constants;

namespace TheBlog.ViewModels.ArticleViewModels
{
    public class UpdateCommentViewModel
    {
        [Required]
        [DisplayName("CommentId")]
        public string CommentId { get; set; }

        [Required]
        [DisplayName("Text")]
        [StringLength(Articles.MaxCommentLength, MinimumLength = Articles.MinCommentLength)]
        public string Text { get; set; }
    }
}
