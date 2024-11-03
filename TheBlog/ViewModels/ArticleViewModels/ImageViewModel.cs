using System.ComponentModel;

namespace TheBlog.ViewModels.ArticleViewModels
{
    public class ImageViewModel
    {
        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("Alt")]
        public string? Alt { get; set; }

        [DisplayName("Href")]
        public string Href { get; set; }
    }
}
