using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TheBlog.Common.Constants;

namespace TheBlog.ViewModels.ArticleViewModels
{
    public class CreateCommentViewModel
    {
        [Required]
        [DisplayName("ArticleId")]
        public string ArticleId { get; set; }

        [Required]
        [DisplayName("Text")]
        [StringLength(Articles.MaxCommentLength, MinimumLength = Articles.MinCommentLength)]
        public string Text { get; set; }
    }
}
