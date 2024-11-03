using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheBlog.ViewModels.ArticleViewModels
{
    public class ArticleViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Text")]
        public string? Text { get; set; }

        [DisplayName("CreationDatetime")]
        public DateTime Created { get; set; }

        [Display(Name = "Text")]
        public string AuthorName { get; set; }

        [DisplayName("Image")]
        public ImageViewModel? Image { get; set; }
    }
}
