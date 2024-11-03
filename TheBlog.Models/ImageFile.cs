using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlog.Models
{
    public class ImageFile
    {
        [ForeignKey("Article")]
        public string? Id { get; set; }

        [Display(Name = "File Name")]
        public string? NameInStorage { get; set; }

        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long? Size { get; set; }

        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime? UploadDateTime { get; set; }

        [NotMapped]
        public byte[]? Bytes { get; set; }

        public virtual Article Article { get; set; }
    }
}
