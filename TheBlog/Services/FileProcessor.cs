using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Net.Mime;
using TheBlog.Common.Constants;
using TheBlog.Interfaces;

namespace TheBlog.Services
{
    public class FileProcessor : IFileProcessor
    {
        public async Task<byte[]> ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState, string[] permittedExtentions, long sizeLimit)
        {
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, ErrorMessages.FileLengthZero);
                return Array.Empty<byte>();
            }

            if (formFile.Length > sizeLimit)
            {
                var megabyteSizeLimit = sizeLimit / 1048576;
                modelState.AddModelError(formFile.Name, ErrorMessages.FileLengthExceedsLimit(megabyteSizeLimit.ToString() + " MB"));
                return Array.Empty<byte>();
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError(formFile.Name, ErrorMessages.FileEmpty);
                        return Array.Empty<byte>();
                    }

                    if (memoryStream.Length > sizeLimit)
                    {
                        var megabyteSizeLimit = sizeLimit / 1048576;
                        modelState.AddModelError(formFile.Name, ErrorMessages.FileLengthExceedsLimit(megabyteSizeLimit.ToString() + " MB"));
                        return Array.Empty<byte>();
                    }

                    if (!IsValidFileName(formFile.FileName, permittedExtentions))
                    {
                        modelState.AddModelError(formFile.FileName, ErrorMessages.FileTypeProhibited);
                        return Array.Empty<byte>();
                    }

                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError("File", $"The upload failed. Error: {ex.HResult}");
            }

            return Array.Empty<byte>();
        }

        public static bool IsValidFileName(string fileName, string[] validExtentions)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var extention = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extention) || !validExtentions.Contains(extention))
            {
                return false;
            }

            return true;
        }
    }
}
