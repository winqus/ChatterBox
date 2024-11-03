using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TheBlog.Models;

namespace TheBlog.Interfaces
{
    public interface IResourceManager
    {
        ImageFile? SaveImage(IFormFile formFile, ModelStateDictionary modelState, string articleId);

        ImageFile? GetImage(string imageId);

        void DeleteImage(ImageFile imageFile);
    }
}
