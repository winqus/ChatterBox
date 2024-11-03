using Microsoft.Extensions.Options;
using System.IO;
using System.Text;
using TheBlog.Interfaces;
using TheBlog.Models;

namespace TheBlog.Services
{
    public class DiskFileManager : IFileManager
    {
        private readonly FileSettings _fileSettings;

        public DiskFileManager(IOptions<FileSettings> options)
        {
            _fileSettings = options.Value;
        }

        public async Task<byte[]> Read(string fileNameInStorage, string storageLocation)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                _fileSettings.StorageRootDirectory,
                storageLocation,
                fileNameInStorage);

            try
            {
                var buffer = await File.ReadAllBytesAsync(path);
                return buffer;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        /// <returns>Saved file name or null.</returns>
        public string? Save(ReadOnlySpan<byte> fileBytes, string storageLocation)
        {
            var fileName = Path.GetRandomFileName() + ".png";
            var dir = Path.Combine(
                Directory.GetCurrentDirectory(),
                _fileSettings.StorageRootDirectory,
                storageLocation);
            var path = Path.Combine(dir, fileName);

            Directory.CreateDirectory(dir);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(fileBytes);
                }
            }
            catch
            {
                return null;
            }

            return fileName;
        }

        public bool Delete(string fileNameInStorage, string storageLocation)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                _fileSettings.StorageRootDirectory,
                storageLocation,
                fileNameInStorage);

            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
