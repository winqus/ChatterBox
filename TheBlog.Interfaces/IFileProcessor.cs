using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace TheBlog.Interfaces
{
    public interface IFileProcessor
    {
        Task<byte[]> ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState, string[] permittedExtentions, long sizeLimit);
    }
}