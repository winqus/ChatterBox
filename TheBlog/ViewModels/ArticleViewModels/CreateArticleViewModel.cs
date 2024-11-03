using System.ComponentModel.DataAnnotations;
using TheBlog.Common.Constants;

namespace TheBlog.ViewModels.ArticleViewModels
{
    public class CreateArticleViewModel
    {
        [Required]
        [StringLength(maximumLength: Articles.MaxTitleLength, MinimumLength = Articles.MinTitleLength)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Text")]
        [StringLength(maximumLength: Articles.MaxTextLength, MinimumLength = Articles.MinTextLength)]
        public string? Text { get; set; }

        [Display(Name = "Image")]
        public IFormFile? Image { get; set; }
    }
}
