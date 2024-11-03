using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBlog.Models
{
    public class FileSettings
    {
        public string StorageRootDirectory { get; set; }

        public string ImageDirectory { get; set; }

        public string[] ValidExtentions { get; set; }

        public long MaxFileSizeInBytes { get; set; }
    }
}
