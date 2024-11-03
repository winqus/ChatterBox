using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.IO;
using TheBlog.Data;
using TheBlog.Interfaces;
using TheBlog.Models;

namespace TheBlog.Services
{
    public class LocalResourceManager : IResourceManager
    {
        private readonly FileSettings _fileSettings;

        private readonly IFileProcessor _fileProcessor;

        private readonly IFileManager _fileManager;

        private readonly IFileData _db;

        public LocalResourceManager(IOptions<FileSettings> options, IFileProcessor fileProcessor, IFileManager fileManager, IFileData db)
        {
            _fileSettings = options.Value;
            _fileProcessor = fileProcessor;
            _fileManager = fileManager;
            _db = db;
        }

        public ImageFile? GetImage(string imageId)
        {
            var image = _db.GetImage(imageId);
            if (image == null)
            {
                return null;
            }

            image.Bytes = _fileManager.Read(image.NameInStorage!, _fileSettings.ImageDirectory).Result;

            if (image.Bytes.Length == 0)
            {
                return null;
            }

            return image;
        }

        public ImageFile? SaveImage(IFormFile formFile, ModelStateDictionary modelState, string articleId)
        {
            byte[] fileBytes = _fileProcessor.ProcessFormFile(formFile, modelState, _fileSettings.ValidExtentions, _fileSettings.MaxFileSizeInBytes).Result;

            if (fileBytes.Length == 0)
            {
                return null;
            }

            var storedFileName = _fileManager.Save(fileBytes, _fileSettings.ImageDirectory);

            if (storedFileName == null)
            {
                return null;
            }

            var oldImage = _db.GetImage(articleId);
            if (oldImage != null)
            {
                DeleteImage(oldImage);
            }

            var image = _db.AddImage(new ImageFile
            {
                Id = articleId,
                NameInStorage = storedFileName,
                Size = fileBytes.Length,
                UploadDateTime = DateTime.Now,
            });

            return image;
        }

        public void DeleteImage(ImageFile imageFile)
        {
            if (imageFile == null)
            {
                return;
            }
            _fileManager.Delete(imageFile.NameInStorage!, _fileSettings.ImageDirectory);
            _db.RemoveImage(imageFile);
        }
    }
}
