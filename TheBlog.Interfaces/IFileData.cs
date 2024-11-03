using TheBlog.Models;

namespace TheBlog.Interfaces
{
    public interface IFileData
    {
        ImageFile? AddImage(ImageFile image);

        ImageFile? GetImage(string id);

        IEnumerable<ImageFile> GetImages();

        void RemoveImage(ImageFile image);
    }
}
