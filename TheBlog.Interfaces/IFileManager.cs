using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBlog.Interfaces
{
    public interface IFileManager
    {
        string? Save(ReadOnlySpan<byte> fileBytes, string storageLocation);

        Task<byte[]> Read(string fileNameInStorage, string storageLocation);

        bool Delete(string fileNameInStorage, string storageLocation);
    }
}
