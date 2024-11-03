using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TheBlog.ViewModels.ArticleViewModels
{
    public class CommentViewModel
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Text")]
        public string Text { get; set; }

        [DisplayName("Author")]
        public string Author { get; set; }

        [DisplayName("CreationDatetime")]
        public DateTime Created { get; set; }
    }
}
